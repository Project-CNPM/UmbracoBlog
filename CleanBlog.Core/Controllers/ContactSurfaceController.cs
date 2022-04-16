using System;
using System.Net.Mail;
using System.Web;
using Umbraco.Web.Mvc;
using System.Web.Mvc;
using CleanBlog.Core.ViewModels;
using Umbraco.Core.Logging;
using Umbraco.Web;
using CleanBlog.Core.Services;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace CleanBlog.Core.Controllers
{
    public class ContactSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {
        private readonly ISmtpService _smtpService;
        private readonly IEmailService _emailService;

        public ContactSurfaceController(ISmtpService smtpService,IEmailService emailService)
        {
            _smtpService = smtpService;
            _emailService = emailService;
        }
        [HttpGet]
        public ActionResult RenderForm()
        {
            ContactViewModel model = new ContactViewModel()
            {
                ContactFormId = CurrentPage.Id
            };
            var siteSettings = Umbraco.ContentAtRoot().DescendantsOrSelfOfType("settings").FirstOrDefault();
            if (siteSettings != null)
            {
                var siteKey = siteSettings.Value<string>("recaptchaSiteKey");
                model.RecaptchaSiteKey = siteKey;
            }
            return PartialView("~/Views/Partials/Contact/contactForm.cshtml", model);
           
        }

        [HttpPost]
        public ActionResult RenderForm(ContactViewModel model)
        {
            return PartialView("~/Views/Partials/Contact/contactForm.cshtml", model);
        }
        [HttpPost]
        public ActionResult SubmitForm(ContactViewModel model)
        {
            bool success = false;
            var contactPage = UmbracoContext.Content.GetById(false, model.ContactFormId);
            var successMessage = contactPage.Value<IHtmlString>("successMessage");
            var errorMessage = contactPage.Value<IHtmlString>("errorMessage");
            try
            {
                
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("Error", "Please check the form.");
                    return PartialView("~/Views/Partials/Contact/result.cshtml", success ? successMessage : errorMessage);
                }
                var siteSettings = Umbraco.ContentAtRoot().DescendantsOrSelfOfType("settings").FirstOrDefault();
                if (siteSettings != null)
                {
                    var secretKey = siteSettings.Value<string>("recaptchaSecretKey");
                    var isCaptchaValid = IsCaptchaValid(Request.Form["GoogleCaptchaToken"], secretKey);
                    if (!isCaptchaValid)
                    {
                        ModelState.AddModelError("Captcha", "The captcha is not valid are you human ?");
                        return PartialView("~/Views/Partials/Contact/result.cshtml", success ? successMessage : errorMessage);
                    }
                }
               
                var contactForms = Umbraco.ContentAtRoot().DescendantsOrSelfOfType("contactForms").FirstOrDefault();

                if (contactForms != null)
                {
                    var newContact = Services.ContentService.Create("Contact", contactForms.Id, "contactForm");
                    newContact.SetValue("contactName", model.Name);
                    newContact.SetValue("contactEmail", model.Email);
                    newContact.SetValue("contactMessage", model.Message);
                    Services.ContentService.SaveAndPublish(newContact);
                }
                //send email
                //SendContactFormReceivedEmail(model);
                _emailService.SendContactNotificationToAdmin(model);

                //Return confirmation message to user
                TempData["status"] = "OK";
                success = true;

                /*
                if(ModelState.IsValid)
                {
                    success = _smtpService.SendEmail(model);
                }*/
               
                
            }
            catch (Exception ex)
            {
                Logger.Error<ContactSurfaceController>("There was an error in the contact form submission", ex.Message);
                ModelState.AddModelError("Error", "Sorry there was a problem noting your details. Would you please try agian later");
            }
            return PartialView("~/Views/Partials/Contact/result.cshtml", success ? successMessage : errorMessage);
        }

        private bool IsCaptchaValid(string token, string secretKey)
        {
            //Sending the token to google api
            HttpClient httpClient = new HttpClient();
            var res = httpClient
                .GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}")
                .Result;
            if (res.StatusCode != System.Net.HttpStatusCode.OK)
                return false;
            string jsonRes = res.Content.ReadAsStringAsync().Result;
            dynamic jsonData = JObject.Parse(jsonRes);
            if(jsonData.success != "true")
            {
                return false;
            }
            return true;
        }

        private void SendContactFormReceivedEmail(ContactViewModel model)
        {
            var siteSettings = Umbraco.ContentAtRoot().DescendantsOrSelfOfType("settings").FirstOrDefault();
            if(siteSettings == null)
            {
                throw new Exception("There was no site settings");
            }
            var fromAddress = siteSettings.Value<string>("emailSettingsFromAddress");
            var toAddress = siteSettings.Value<string>("emailSettingsAdminAccounts");
            if(string.IsNullOrEmpty(fromAddress))
            {
                throw new Exception("There needs to be a from address in site settings");
            }
            if(string.IsNullOrEmpty(toAddress))
            {
                throw new Exception("There needs to be a from address in site settings");
            }
            var emailSubject = "There has been a contact form submitted";
            var emailBody = $"A new contact form has been received from {model.Name}. Their message were: {model.Message}";
            var smtpMessage = new MailMessage();
            smtpMessage.Subject = emailSubject;
            smtpMessage.Body = emailBody;
            smtpMessage.From = new MailAddress(fromAddress);
            
            var toList = toAddress.Split(',');
            foreach (var item in toList)
            {
                if (!string.IsNullOrEmpty(item))
                    smtpMessage.To.Add(item);
            }

            using (var smtp = new SmtpClient())
            {
                smtp.Send(smtpMessage);
            }
        }
    }
}
