using System.Net.Mail;
using emailSend.Models;
using Microsoft.AspNetCore.Mvc;

namespace emailSend.Controllers;

public class EmailSenderController : Controller
{
    public IActionResult EmailForm()
    {
        return View();
    }
    public IActionResult SendEmail(MailModel mailModel)
    {
        // Send email
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("ayushrudani09@gmail.com");
            mail.To.Add(mailModel.To);
            mail.Subject = mailModel.Subject;
            mail.Body = mailModel.Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("ayushrudani09@gmail.com", "pmxn hhdu gjwy chmn"); // Enter seders User name and password  
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            ViewBag.Error = e.Message;
            return View("EmailForm");
        }
        return RedirectToAction("EmailForm");
    }
}

//  The  SendEmail  method is the endpoint that will be called to send an email. It takes an  EmailRequest  object as a parameter, which is a simple class that contains the email's subject, body, and recipient. 
//  The  SendEmail  method calls the  SendEmailAsync  method of the  IEmailSender  interface, which is injected into the controller's constructor. The  SendEmailAsync  method is an asynchronous method that sends an email using the email service. 
//  The  SendEmail  method returns an  IActionResult  object with an  Ok  status code. This indicates that the email was sent successfully. 
//  The  EmailRequest  class is a simple class that contains the email's subject, body, and recipient.

