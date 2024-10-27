# Payroll System Documentation

## Overview

The Payroll System is designed to efficiently manage employee payroll and related functions, ensuring streamlined operations and accurate record-keeping. The system encompasses various features, including employee management, attendance tracking, department management, salary management, incentive management, and detailed reporting. Built with modern technologies, the system is aimed at delivering reliability and high performance.

## Technologies Used

- **.NET 8**: The primary framework utilized for building the application, ensuring robust performance and scalability.
- **SQL Server**: A powerful relational database management system used for reliable data storage and complex querying capabilities.
- **Entity Framework Core (EF Core)**: An ORM framework that simplifies data access and manipulation while promoting best practices in database interaction.
- **In-Memory Cache**: Utilized for quick access to frequently requested data, improving response times and performance.
- **Repository Pattern**: Implements an abstraction layer over data access, promoting a clean architecture and easier testing.
- **Unit of Work Pattern**: Manages database transactions in a coherent manner, ensuring data integrity and consistency during operations.
- **MVC (Model-View-Controller)**: A design pattern that separates application logic into three interconnected components, improving code organization and maintainability.

## Features

- **Employee Management**: 
  - Manage comprehensive employee records, including personal information, hire dates, roles, and contact details.
  
- **Attendance Tracking**: 
  - A script named `Fill_attendance_records` is available to generate attendance records for employees starting from their hire date, ensuring no duplication of entries.
  
- **Department Management**: 
  - Organize employees into various departments for enhanced management and reporting, facilitating targeted oversight and resource allocation.
  
- **Salary Management**: 
  - Calculate and manage employee salaries with features to handle different pay structures, ensuring accuracy in payroll processing.
  
- **Incentive Management**: 
  - Manage and distribute performance-based incentives to enhance employee engagement and retention, including customizable incentive programs.
  
- **Inductive Management**: 
  - Implement strategies aimed at boosting employee engagement and productivity through various motivational initiatives.
  
- **Reporting**: 
  - Generate comprehensive reports covering payroll, attendance, and departmental performance metrics for informed decision-making.

## Getting Started

### Prerequisites

Before you begin, ensure you have the following installed:

- **.NET 8 SDK**: Download from the official [.NET website](https://dotnet.microsoft.com/download).
- **SQL Server**: Ensure that you have SQL Server installed and running. You can use SQL Server Express for development purposes.
- **Code Editor**: Any code editor, such as Visual Studio or Visual Studio Code, is recommended for development.

### Configuration

1. **Database Connection**:
   - Modify the `connectionString` in `appSettings.json` and `appSettings.Development.json` to match your SQL Server connection requirements.

2. **Database Initialization**:
   - When running the project in debug mode, the system will automatically apply migrations to your database to set up the necessary schema.

3. **Scripts and Backups**:
   - A script named `Fill_attendance_records` is located in the `Database_Stuffs` folder. This script allows you to populate attendance records for any employee from their hire date.
   - You can also find a full database backup called `Payroll_db` in the `Database_Stuffs` folder if you wish to test the system with pre-existing data.
