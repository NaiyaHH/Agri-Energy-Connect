
# Agri Energy Connect
This is a farming e-commerce web application for the purpose of connecting farmers, green energy experts, and enthusiasts; allowing farmers to apply to register,  to add an unlimited number of products within the categories provided, and get in touch with other account-holders who advertise products. It is formatted as an ASP.NET MVC (Model-View-Controller) application, written in C# (pronounced “C-Sharp”) and was created using Visual Studio 2022 (which would also be required to run this application) it uses ADO.NET, .NET 8, Entity Framework Core, Firebase Authentication and the database management application used for database creation is SQL Server Management Studio 2019. 


## TABLE OF CONTENTS
- [Key Features](#key-features)
- [Version](#version)
- [Assumptions](#assumptions)
- [Video Tutorial Link](#video-tutorial-link)
- [Installation and Running of the Application](#installation-and-running-of-the-application)
   + [Requirements to install the project](#requirements-to-install-the-project)
   + [How to install and run the application](#how-to-install-and-run-the-application)
- [Login Credentials](#login-credentials)
- [FAQ](#faq)
- [License](#license)
- [References](#references)


## Key Features
- There are 4 roles within this application (additional features not required by the POE are marked with a *)
  1. Admin
  2. Farmer
  3. Denied*
  4. Requested*
- An admin can:
  1. Log in
  2. Register a new admin user
  3. View All Users
  4. View All Products
     a. Filter by Date Range
     b. Filter by Farmer Name
     c. Filter by Category
     d. Filter by Product Name
  6. Edit All Products*
  7. View All Categories with images
  8. Add a Category
- A farmer can:
  1. Log In
  2. Add a Product
  3. Edit their own Products*
  4. View All Products with images*
     a. Filter by Date Range
     b. Filter by Farmer Name
     c. Filter by Category using a drop down box
     d. Filter by Product Name
  5. Contact other farmers regarding the purchase of Products*
- A user who does not have an account can request to register as a farmer.
  1. Once the user requests to register as a farmer, their user role is automatically "Requested" and until an admin does not approve the user's request, their user role will remain "Requested".
  2. A user whose role is "Requested" only has access to a view that displays a message explaining that their application is being processed and they should kindly be patient.*
  3. If an admin sees fit, they can change a user's role to "Denied", where the user will only be able to access a view that informs them that their application has been rejected.*

## Version
2.0

## Assumptions
- Each farmer can see other farmers products - this facilitates connection between farmers.
- Each farmer can see the product categories - this will provide context for the farmers when they list products.
- Farmers can contact other farmers through Agri-Energy Connect to do business - this ensures that farmers do not have to pay commission (keeping all their profits) and that farmers can ensure that they are connecting with reliable people as the farmers who have accounts with Agri-Energy Connect are thoroughly screened.
- There should not be product images - as this may be misleading for farmers who are looking to purchase certain items, for example, a farmer may find an image of apples on the internet and upload that picture, while the apples that they are selling actually look quite different.
- Admins can edit products that farmers have listed - in the case where farmers may make mistakes when listing products and they either do not want to or do not know how to edit their products themselves.

## Video Tutorial Link

https://youtu.be/aK6YH4mLXn4 


## Installation and running of the Application
### Requirements to install the project
1. Microsoft Visual Studio 2022
2. A computer device with at least an Intel Celeron Processor, 4GB of RAM and an Internet connection.
3. A GitHub account (to clone the repository).
4. SQL Server Management Studio 19
5. The database script

### How to install and run the application:
1. Go to https://visualstudio.microsoft.com/vs/ and click 'Download'.
2. Click on 'Community 2022'.
3. Double click the 'VisualStudioSetup.exe' file.
4. Follow the prompts to download Microsoft Visual Studio.
5. Once you have Microsoft Visual Studio set up, go to https://github.com/
6. Sign up or sign into GitHub.
7. Go to https://github.com/VCDN-2024/prog7311-poe-ST10033475
8. Copy the following link: https://github.com/VCDN-2024/prog7311-poe-ST10033475 (by pressing ‘CTRL’ and ‘C’ at the same time on your keyboard).
9. Go back to Visual Studio, and click on ‘Clone a Repository’.
10. Paste the link where you are prompted to paste link (by pressing ‘CTRL’ and ‘V’ at the same time on your keyboard).
11. Press ‘ENTER’ to clone the repository.
12. Run the entire database script in SSMS 19 and replace the current connection string with your own connection string
13. To run the application, click on the green triangle or Play button that is located at the middle top of the screen to start the application.

## Login Credentials
Admin Email Address: admin1@agrienergyconnect.com
Admin Password: poiuytrewq
Farmer Email Address: yusraadnan25@gmail.com
Farmer Password: poiuytrewq

## FAQ
### How long does it take for my request to register as a farmer take?
Our dedicated employees will either approve or decline your request within a week of your request.

### How would a farmer get in contact with me if they were interested in one of my products?
A farmer can send you a message through the Agri-Energy Connect platform, which you will recieve as an email; thereafter, you and your potential client can discuss as you prefer. 

## License
MIT License - [LICENSE](LICENSE)

## References
Adam, Ebrahim. 2023. MathApp, 26 May 2024. [Online] Available at: https://github.com/PROG7311-VCDN-2024/MathApp [Accessed 12 April 2024].

Bootswatch, 2024. Bootswatch: Free themes for bootstrap, 2024. [Online] Available at: https://bootswatch.com/litera/ [30 May 2024].

Heidi, Erika. 2022. Documentation 101: creating a good README for your software project, 14 December 2022. [Online]. Available at: https://dev.to/erikaheidi/documentation-101-creating-a-good-readme-for-your-software-project-cf8 [Accessed 22 September 2023].

Leshaba, Isaac. 11 June 2023. ASP.NET Core MVC Web app Using Entity Framework. [Online]. Available at: https://drive.google.com/file/d/1JdVWfbfYmFYh5xd7TcG6djbUBRNo_Y3F/view [Accessed 20 June 2023].
