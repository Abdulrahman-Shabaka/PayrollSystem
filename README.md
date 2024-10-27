# Payroll System

## Overview

This Payroll System is designed to manage employee payroll efficiently. It includes features such as employee management, attendance tracking, department management, salary management, incentive management, and reporting. Built with modern technologies, it ensures reliability and performance.

## Technologies Used

- **.NET 8**: The core framework used to build the application.
- **SQL Server**: For robust data storage and management.
- **Entity Framework Core (EF Core)**: To simplify data access and manipulation.
- **In-Memory Cache**: For quick access to frequently used data.
- **Repository Pattern**: To abstract data access and promote a clean architecture.
- **Unit of Work Pattern**: To manage transactions and ensure consistency.
- **MVC (Model-View-Controller)**: To separate concerns and organize the application structure.

## Features

- **Employee Management**: Manage employee records, including hire dates and personal information.
- **Attendance Tracking**: A script that generates attendance records for existing or new employees from their hire date with no duplication.
- **Department Management**: Organize employees into various departments for better management and reporting.
- **Salary Management**: Calculate and manage employee salaries effectively.
- **Incentive Management**: Manage incentives for employees to boost performance and retention.
- **Inductive Management**: Implement strategies to encourage employee engagement and productivity.
- **Reporting**: Generate comprehensive reports for payroll, attendance, and other management areas.

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- Any code editor (e.g., Visual Studio, Visual Studio Code)

### Notes

1. You can find a script called "Fill_attendace_records" for filling attendance records for any employee scince he get hired in folder called Database_Stuffs.
2. Also ther is a Full Db Backup called "Payroll_db" if you want to test with it in the folder Database_Stuffs.
