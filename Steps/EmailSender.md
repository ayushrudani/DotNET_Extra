# Email Sender

## Description

This module is responsible for sending emails to the users. It uses the SMTP protocol to send emails.

## Functions

### send_email

This function sends an email to the user.

#### Parameters

- `email`: The email address of the user.
- `subject`: The subject of the email.
- `message`: The message of the email.

#### Returns

- `bool`: True if the email was sent successfully, False otherwise.

## How to do this using C# and .NET Core MVC

### Step 1: Install the `System.Net.Mail` package

```bash
dotnet add package System.Net.Mail
```

### Step 2: Create the `EmailSender` Controller

#### How to genrate App Password

- Go to your Google Account.
- On the left navigation panel, click Security.
- On the Signing in to Google panel, click App passwords. If you don't see this option:
  - 2-Step Verification is not set up for your account.
  - 2-Step Verification is set up for security keys only.
  - Your account is through work, school, or other organization.
- At the bottom, click Select app and choose the app you’re using.
- Click Select device and choose the device you’re using.
- Click Generate.
- Follow the instructions to enter the App password (the 16 character code in the yellow bar) on your device.
- Click Done.

```csharp
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
            mail.From = new MailAddress("your_email");
            mail.To.Add(mailModel.To);
            mail.Subject = mailModel.Subject;
            mail.Body = mailModel.Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("your_email", "your_app_password"); // Enter seders User name and password
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
```

### Step 3: Create the `MailModel` class

```csharp
public class MailModel
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
```

### Step 4: Create the `EmailForm` View

```html
@model MailModel

<form asp-action="SendEmail" method="post">
  <div class="form-group">
    <label for="To">To:</label>
    <input asp-for="To" class="form-control" />
  </div>
  <div class="form-group">
    <label for="Subject">Subject:</label>
    <input asp-for="Subject" class="form-control" />
  </div>
  <div class="form-group">
    <label for="Body">Body:</label>
    <textarea asp-for="Body" class="form-control"></textarea>
  </div>
  <button type="submit" class="btn btn-primary">Send Email</button>

  @if (ViewBag.Error != null) {
  <p class="text-danger">@ViewBag.Error</p>
  }
</form>
```

### Step 5: Run the application

```bash

dotnet run
```

### Step 6: Open the browser and navigate to `https://localhost:5001/EmailSender/EmailForm`

```
## Additional Information

- You can use any email provider to send emails. Just change the SMTP server and port accordingly.
- You can also use environment variables to store sensitive information like email and password.
```
