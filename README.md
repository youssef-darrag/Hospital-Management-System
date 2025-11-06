# 🏥 Hospital Management System

A **comprehensive web-based Hospital Management System** built with **ASP.NET Core MVC** that streamlines hospital operations, patient management, and medical workflows.  
This system provides an efficient platform for managing patients, doctors, appointments, and medical records in a healthcare environment.

---

## 📚 Table of Contents
- [🚀 Features](#-features)
- [💻 Technologies Used](#-technologies-used)
- [📁 Project Structure](#-project-structure)
- [🔧 Installation & Setup](#-installation--setup)
- [🎯 Key Implementation Details](#-key-implementation-details)
- [🌟 Professional Skills Demonstrated](#-professional-skills-demonstrated)
- [📊 Future Enhancements](#-future-enhancements)
- [📄 License](#-license)
- [🤝 Contributing](#-contributing)

---

## 🚀 Features

### 🔐 Authentication & Authorization
- Role-based access control with **ASP.NET Core Identity**
- Secure user registration and login
- Custom authorization policies for different user types

### 🏥 Core Functionalities
- **Patient Management:** Full patient profiles, medical history, and records  
- **Doctor Management:** Schedules, specialties, and availability  
- **Appointment System:** Book, reschedule, and manage appointments  
- **Medical Records:** Securely store and manage patient data  
- **Department Management:** Organize hospital departments and services  

### 💻 Technical Features
- Responsive UI with **Bootstrap 5**  
- Data access via **Entity Framework Core**  
- **Repository Pattern** and **Dependency Injection** for clean architecture  
- **MVC Architecture** ensuring separation of concerns  
- Integrated **SQL Server Database**  
- Dynamic rendering with **Razor Pages**

---

## 💻 Technologies Used

### 🧠 Backend
- **ASP.NET Core 9.0** – Web framework  
- **Entity Framework Core** – ORM  
- **ASP.NET Core Identity** – Authentication  
- **Dependency Injection** – Loosely coupled architecture  
- **Repository Pattern** – Data abstraction layer  

### 🎨 Frontend
- **Bootstrap 5** – Responsive design  
- **Razor Syntax** – Server-side rendering  
- **HTML5 / CSS3 / JavaScript** – Client-side UI  

### 🗄 Database
- **SQL Server** – Primary database  
- **EF Core Migrations** – Database versioning and schema management  

---

## 📁 Project Structure
   ```text
   Hospital-Management-System/
   │
   ├── 📁 Hospital.Core/                                # Core Domain Layer
   │   │
   │   ├── 📁 Consts/                                   # Constant Files
   │   │   ├── ImagePaths.cs
   │   │   ├── OrderBy.cs
   │   │   └── WebSiteRoles.cs
   │   │
   │   ├── 📁 Helpers/                                  # Helper Classes
   │   │   ├── EmailSender.cs
   │   │   ├── GenericResponse.cs
   │   │   ├── IDbInitializer.cs
   │   │   ├── ImageOperation.cs
   │   │   └── PagedResult.cs
   │   │
   │   ├── 📁 Hubs/                                     # Hubs
   │   │   └── NotificationHub.cs
   │   │
   │   ├── 📁 Models/                                   # Entity Models
   │   │   ├── ApplicationUser.cs
   │   │   ├── Appointment.cs
   │   │   ├── Bill.cs
   │   │   ├── Contact.cs
   │   │   ├── Department.cs
   │   │   ├── HospitalInfo.cs
   │   │   ├── Insurance.cs
   │   │   ├── Lab.cs
   │   │   ├── Medicine.cs
   │   │   ├── MedicineReport.cs
   │   │   ├── Notification.cs
   │   │   ├── PatientReport.cs
   │   │   ├── Payroll.cs
   │   │   ├── PrescribedMedicine.cs
   │   │   ├── Room.cs
   │   │   ├── Supplier.cs
   │   │   ├── TestPrice.cs
   │   │   └── Timing.cs
   │   │
   │   ├── 📁 Repositories/                             # Repository Interfaces
   │   │   ├── IGenericRepository.cs
   │   │   └── IUnitOfWork.cs
   │   │
   │   ├── 📁 Services/                                 # Service Interfaces
   │   │   ├── IApplicationUserService.cs
   │   │   ├── IAppointmentService.cs
   │   │   ├── IContactService.cs
   │   │   ├── IDoctorService.cs
   │   │   ├── IHospitalInfoService.cs
   │   │   ├── INotificationService.cs
   │   │   ├── IRoomService.cs
   │   │   └── ITimingService.cs
   │   │
   │   ├── 📁 Settings/                                 # Settings
   │   │   └── FileSettings.cs
   │   │
   │   ├── 📁 ViewModels/                               # View Models
   │   │   ├── ApplicationUserViewModel.cs
   │   │   ├── BookAppointmentViewModel.cs
   │   │   ├── ContactViewModel.cs
   │   │   ├── DoctorViewModel.cs
   │   │   ├── HospitalInfoViewModel.cs
   │   │   ├── NotificationsViewModel.cs
   │   │   ├── RoomViewModel.cs
   │   │   ├── SlotViewModel.cs
   │   │   └── TimingViewModel.cs
   │   │
   │   └── 📄 Hospital.Core.csproj
   │
   ├── 📁 Hospital.EF/                                  # Data Access Layer
   │   │
   │   ├── 📁 Helpers/                                  # Helper Classes
   │   │   ├── DbInitializer.cs
   │   │   └── NameIdentifierUserIdProvider.cs
   │   │
   │   ├── 📁 Migrations/                               # EF Core Migrations
   │   │
   │   ├── 📁 Repositories/                             # Repository Implementations
   │   │   ├── GenericRepository.cs
   │   │   └── UnitOfWork.cs
   │   │
   │   ├── 📁 Services/                                 # Service Implementations
   │   │   ├── ApplicationUserService.cs
   │   │   ├── AppointmentService.cs
   │   │   ├── ContactService.cs
   │   │   ├── DoctorService.cs
   │   │   ├── HospitalInfoService.cs
   │   │   ├── NotificationService.cs
   │   │   ├── RoomService.cs
   │   │   └── TimingService.cs
   |   │
   │   ├── ApplicationDbContext.cs
   │   └── Hospital.EF.csproj
   │
   ├── 📁 Hospital.Web/                                # Web API Layer
   │   │
   │   ├── 📁 Areas/
   │   │   ├── 📁 Admin/
   │   │   │   ├── 📁 Controllers/                     # API Controllers
   │   │   │   │   ├── ApplicationUsersController.cs
   │   │   │   │   ├── AppointmentsController.cs
   │   │   │   │   ├── ContactsController.cs
   │   │   │   │   ├── DoctorsController.cs
   │   │   │   │   ├── HomeController.cs
   │   │   │   │   ├── HospitalsController.cs
   │   │   │   │   ├── NotificationsController.cs
   │   │   │   │   ├── RoomsController.cs
   │   │   │   │   └── TimingsController.cs
   │   │   │   │
   │   │   │   ├── 📁 Views/                           # Views
   │   │   │   │   ├── 📁 ApplicationUsers/
   │   │   │   │   ├── 📁 Appointments/
   │   │   │   │   ├── 📁 Contacts/
   │   │   │   │   ├── 📁 Doctors/
   │   │   │   │   ├── 📁 Home/
   │   │   │   │   ├── 📁 Hospitals/
   │   │   │   │   ├── 📁 Notifications/
   │   │   │   │   ├── 📁 Rooms/
   │   │   │   │   └── 📁 Timings/
   │   │   │
   │   │   ├── 📁 Identity/Pages/
   │   │   │
   │   │   ├── _ViewImports.cshtml
   │   │   └── _ViewStart.cshtml
   │   │
   │   ├── 📁 Properties/
   │   │   └️ launchSettings.json
   │   │
   │   ├── 📁 wwwroot/
   │   │
   │   ├── Hospital.Web.csproj
   │   ├── Program.cs
   │   ├── appsettings.Development.json
   │   ├── appsettings.json
   │   └── libman.json
```
---

## 🔧 Installation & Setup

### 🧩 Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### ⚙ Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/youssef-darrag/Hospital-Management-System.git

2. Configure the database connection
Update the connection string in appsettings.json.

3. Apply migrations
   ```bash
   dotnet ef database update

4. Run the application
   ```bash
   dotnet run

---

## 🎯 Key Implementation Details

### 🏗 Architecture Patterns
- **MVC Pattern** – Separation between Model, View, and Controller
- **Repository Pattern** – Abstracted data access
- **Dependency Injection** – Inversion of control for better testability

### 🔒 Security Features
- Password hashing & salting
- Role-based authorization
- Secure session management
- Input validation and sanitization

### 🗃 Database Design
- Normalized schema
- Defined relationships between entities
- Indexed columns for performance
- Foreign key constraints for integrity

---

## 🌟 Professional Skills Demonstrated

### 👨‍💻 Backend Development
- ASP.NET Core MVC
- Entity Framework Core
- Authentication & Authorization
- RESTful API Design
- Database optimization

### 💅 Frontend Development
- Responsive design with Bootstrap
- Client-side validation
- AJAX integration
- Enhanced UX/UI

### 🧩 Software Engineering
- Clean code & architecture
- Version control (Git)
- Debugging & optimization
- Scalable project design

---

## 📊 Future Enhancements

- Real-time notifications
- Advanced reporting and analytics
- Mobile app integration
- AI-powered appointment suggestions
- Telemedicine features

---

## 📄 License

This project is licensed under the [MIT License](/blob/main/LICENSE) – see the LICENSE file for details.

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome!
Feel free to check the [issues page](/issues) and submit a pull request.
