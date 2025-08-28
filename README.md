# 👨‍💼 Employee Management System (ASP.NET MVC)

## 📌 Overview
The **Employee Management System** is a web application built using **ASP.NET MVC** and follows a **3-Tier Architecture**.  
It demonstrates modern software engineering practices including **Generic Repository Design Pattern**, **Unit of Work**, **AutoMapper**, and **manual mapping with Extension Methods**.  

The system allows seamless management of **Employees** and **Departments**, providing CRUD operations with a clean separation of concerns.

---

## ✨ Features
### 🔹 Core Modules
- **Department Module**
  - Create, update, delete, and view departments
  - Assign employees to departments

- **Employee Module**
  - Create, update, delete, and view employees
  - Link employees with departments

### 🔹 Architecture & Patterns
- **3-Tier Architecture**
  - **Presentation Layer (MVC)**: Controllers, Views, ViewModels
  - **Business Layer (Services)**: DTOs, business rules
  - **Data Access Layer (DAL)**: EF Core + Repositories

- **Design Patterns**
  - **Generic Repository Pattern**
  - **Unit of Work**
  - **DTOs for clean data transfer**

- **Mapping**
  - **AutoMapper** (automatic object mapping)
  - **Manual Mapping** using **Extension Methods**

---

## 🛠️ Technical Implementation
- **Controllers**
  - `DepartmentsController`
  - `EmployeesController`
  - `AccountController` (Authentication)  
  - `RolesController` (Authorization)

- **Views**
  - Strongly-typed Razor Views
  - **Partial Views** for reusable UI
  - **ViewData**, **ViewBag**, and **TempData** for passing data between controllers and views

- **DTOs**
  - `EmployeeCreateRequest`
  - `EmployeeResponse`
  - `EmployeeUpdateRequest`
  - `EmployeeDetailsResponse`

  - `DepartmentResponse`
  - `DepartmentDetailsResponse`
  - `DepartmentCreateRequest`
  - `DepartmentUpdateRequest`
  
- **Authentication**
  - `AccountController` handles login, logout, reset password, and registration
