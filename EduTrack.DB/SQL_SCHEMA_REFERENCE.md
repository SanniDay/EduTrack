# EduTrack - SQL Schema Reference Guide

## Overview
This document provides a comprehensive reference for all database tables and their stored procedures in the EduTrack project.

---

## Table Reference Guide

### 1. Classes Table
**Full Name**: `dbo.Classes`
**Primary Key**: `Class_Id` (INT, IDENTITY)

**Columns**:
| Column | Type | Nullable | Default | Notes |
|--------|------|----------|---------|-------|
| Class_Id | INT | NO | IDENTITY(1,1) | Primary Key |
| ClassName | NVARCHAR(100) | NO | - | Class name/label |
| Section | NVARCHAR(50) | NO | - | Section identifier |
| **IsActive** | BIT | NO | 1 | ✅ **PascalCase** |
| **IsDeleted** | BIT | NO | 0 | ✅ **PascalCase** - Soft delete flag |
| Created_By | NVARCHAR(100) | NO | - | Audit trail |
| Created_Date | DATETIME | NO | getdate() | Audit trail |
| Modified_By | NVARCHAR(100) | YES | - | Audit trail |
| Modified_Date | DATETIME | YES | - | Audit trail |

**Stored Procedures**:
- `sp_Class_GetAll` - Get all non-deleted classes
- `sp_Class_GetById` - Get class by ID
- `sp_Class_Create` - Create new class
- `sp_Class_Update` - Update class details
- `sp_Class_Delete` - Soft delete a class

**Indexes**:
- `IX_Classes_IsDeleted` on `IsDeleted` (ASC)
- `IX_Classes_IsActive` on `IsActive` (ASC)

---

### 2. Students Table
**Full Name**: `dbo.Students`
**Primary Key**: `Student_Id` (INT, IDENTITY)

**Columns**:
| Column | Type | Nullable | Default | Notes |
|--------|------|----------|---------|-------|
| Student_Id | INT | NO | IDENTITY(1,1) | Primary Key |
| User_Id | INT | NO | - | FK to Users table |
| FullName | VARCHAR(100) | NO | - | Student full name |
| DOB | DATE | YES | - | Date of birth |
| Gender | VARCHAR(10) | YES | - | Gender |
| **isActive** | BIT | NO | 0 | ⚠️ **camelCase** |
| **isDeleted** | BIT | NO | 0 | ⚠️ **camelCase** - Soft delete flag |
| Created_By | VARCHAR(50) | NO | - | Audit trail |
| Created_Date | DATETIME | NO | getdate() | Audit trail |
| Modified_By | VARCHAR(50) | NO | - | Audit trail |
| Modified_Date | DATETIME | NO | getdate() | Audit trail |

**Foreign Keys**:
- `FK_Students_User` → Users(User_Id) ON DELETE CASCADE
- `UQ__Students__206D9171A4561648` - Unique on User_Id

---

### 3. Teachers Table
**Full Name**: `dbo.Teachers`
**Primary Key**: `Teacher_Id` (INT, IDENTITY)

**Columns**:
| Column | Type | Nullable | Default | Notes |
|--------|------|----------|---------|-------|
| Teacher_Id | INT | NO | IDENTITY(1,1) | Primary Key |
| User_Id | INT | NO | - | FK to Users table |
| FullName | VARCHAR(100) | NO | - | Teacher full name |
| **isActive** | BIT | NO | 0 | ⚠️ **camelCase** |
| **isDeleted** | BIT | NO | 0 | ⚠️ **camelCase** - Soft delete flag |
| Created_By | VARCHAR(50) | NO | - | Audit trail |
| Created_Date | DATETIME | NO | getdate() | Audit trail |
| Modified_By | VARCHAR(50) | NO | - | Audit trail |
| Modified_Date | DATETIME | NO | getdate() | Audit trail |

**Foreign Keys**:
- `FK_Teachers_User` → Users(User_Id) ON DELETE CASCADE

---

### 4. Subjects Table
**Full Name**: `dbo.Subjects`
**Primary Key**: `Subject_Id` (INT, IDENTITY)

**Columns**:
| Column | Type | Nullable | Default | Notes |
|--------|------|----------|---------|-------|
| Subject_Id | INT | NO | IDENTITY(1,1) | Primary Key |
| Subject_Name | NVARCHAR(100) | NO | - | Subject name |
| Subject_Code | NVARCHAR(20) | NO | - | Unique subject code |
| Description | NVARCHAR(MAX) | YES | - | Subject description |
| **IsActive** | BIT | NO | 1 | ✅ **PascalCase** |
| **IsDeleted** | BIT | NO | 0 | ✅ **PascalCase** - Soft delete flag |
| Created_By | NVARCHAR(100) | NO | - | Audit trail |
| Created_Date | DATETIME | NO | getdate() | Audit trail |
| Modified_By | NVARCHAR(100) | YES | - | Audit trail |
| Modified_Date | DATETIME | YES | - | Audit trail |

**Indexes**:
- `IX_Subjects_IsDeleted` on `IsDeleted` (ASC)
- `IX_Subjects_IsActive` on `IsActive` (ASC)

---

### 5. Fees Table
**Full Name**: `dbo.Fees`
**Primary Key**: `Fees_Id` (INT, IDENTITY)

**Columns**:
| Column | Type | Nullable | Default | Notes |
|--------|------|----------|---------|-------|
| Fees_Id | INT | NO | IDENTITY(1,1) | Primary Key |
| Class_Id | INT | NO | - | FK to Classes table |
| FeeType | INT | NO | - | Type of fee |
| Amount | DECIMAL(10,2) | NO | - | Fee amount |
| Currency | NVARCHAR(3) | NO | 'USD' | Currency code |
| Description | NVARCHAR(MAX) | YES | - | Fee description |
| DueDate | DATETIME | YES | - | Payment due date |
| **IsActive** | BIT | NO | 1 | ✅ **PascalCase** |
| **IsDeleted** | BIT | NO | 0 | ✅ **PascalCase** - Soft delete flag |
| Created_By | NVARCHAR(100) | NO | - | Audit trail |
| Created_Date | DATETIME | NO | getdate() | Audit trail |
| Modified_By | NVARCHAR(100) | YES | - | Audit trail |
| Modified_Date | DATETIME | YES | - | Audit trail |

**Foreign Keys**:
- `FK_Fees_Class` → Classes(Class_Id)

**Unique Constraints**:
- `UC_Fees_ClassType` on (Class_Id, FeeType)

---

### 6. Roles Table
**Full Name**: `dbo.Roles`
**Primary Key**: `Role_Id` (INT, IDENTITY)

**Columns**:
| Column | Type | Nullable | Default | Notes |
|--------|------|----------|---------|-------|
| Role_Id | INT | NO | IDENTITY(1,1) | Primary Key |
| Role_Name | VARCHAR(50) | NO | - | Unique role name |
| **isActive** | BIT | YES | 0 | ⚠️ **camelCase** |
| **isDeleted** | BIT | YES | 0 | ⚠️ **camelCase** - Soft delete flag |
| Created_By | VARCHAR(50) | NO | - | Audit trail |
| Created_Date | DATETIME | YES | getdate() | Audit trail |
| Modified_By | VARCHAR(50) | NO | - | Audit trail |
| Modified_Date | DATETIME | YES | getdate() | Audit trail |

---

### 7. StudentClass Table
**Full Name**: `dbo.StudentClass`
**Primary Key**: `Student_Class_Id` (INT, IDENTITY)

**Columns**:
| Column | Type | Nullable | Default | Notes |
|--------|------|----------|---------|-------|
| Student_Class_Id | INT | NO | IDENTITY(1,1) | Primary Key |
| Student_Id | INT | NO | - | FK to Students table |
| Class_Id | INT | NO | - | FK to Classes table |
| **isActive** | BIT | NO | 1 | ⚠️ **camelCase** |
| **isDeleted** | BIT | NO | 0 | ⚠️ **camelCase** - Soft delete flag |
| Created_By | VARCHAR(50) | NO | - | Audit trail |
| Created_Date | DATETIME | NO | getdate() | Audit trail |
| Modified_By | VARCHAR(50) | NO | - | Audit trail |
| Modified_Date | DATETIME | NO | getdate() | Audit trail |

---

### 8. TeacherClass Table
**Full Name**: `dbo.TeacherClass`
**Primary Key**: `Teacher_Class_Id` (INT, IDENTITY)

**Columns**:
| Column | Type | Nullable | Default | Notes |
|--------|------|----------|---------|-------|
| Teacher_Class_Id | INT | NO | IDENTITY(1,1) | Primary Key |
| Teacher_Id | INT | NO | - | FK to Teachers table |
| Class_Id | INT | NO | - | FK to Classes table |
| ClassSubject_Id | INT | YES | - | FK to ClassSubject table (NEW) |
| **isActive** | BIT | NO | 1 | ⚠️ **camelCase** |
| **isDeleted** | BIT | NO | 0 | ⚠️ **camelCase** - Soft delete flag |
| Created_By | VARCHAR(50) | NO | - | Audit trail |
| Created_Date | DATETIME | NO | getdate() | Audit trail |
| Modified_By | VARCHAR(50) | NO | - | Audit trail |
| Modified_Date | DATETIME | NO | getdate() | Audit trail |

**Foreign Keys**:
- `FK_TeacherClass_CS` → ClassSubject(ClassSubject_Id)

**Unique Constraints**:
- `UX_TeacherClass` on (Teacher_Id, ClassSubject_Id) WHERE isDeleted=0

---

### 9. ClassSubject Table (NEW)
**Full Name**: `dbo.ClassSubject`
**Primary Key**: `ClassSubject_Id` (INT, IDENTITY)

**Columns**:
| Column | Type | Nullable | Default | Notes |
|--------|------|----------|---------|-------|
| ClassSubject_Id | INT | NO | IDENTITY(1,1) | Primary Key |
| Class_Id | INT | NO | - | FK to Classes table |
| Subject_Id | INT | NO | - | FK to Subjects table |
| IsCore | BIT | YES | 1 | Is core subject |
| **IsActive** | BIT | YES | 1 | ✅ **PascalCase** |
| **IsDeleted** | BIT | YES | 0 | ✅ **PascalCase** - Soft delete flag |
| Created_By | NVARCHAR(100) | YES | - | Audit trail |
| Created_Date | DATETIME | YES | getdate() | Audit trail |
| Modified_By | NVARCHAR(100) | YES | - | Audit trail |
| Modified_Date | DATETIME | YES | - | Audit trail |

**Unique Constraints**:
- `UX_ClassSubject` on (Class_Id, Subject_Id) WHERE IsDeleted=0

---

### 10. Attendance Table (NEW)
**Full Name**: `dbo.Attendance`
**Primary Key**: `Attendance_Id` (INT, IDENTITY)

**Columns**:
| Column | Type | Nullable | Default | Notes |
|--------|------|----------|---------|-------|
| Attendance_Id | INT | NO | IDENTITY(1,1) | Primary Key |
| Student_Class_Id | INT | NO | - | FK to StudentClass table |
| ClassSubject_Id | INT | YES | - | FK to ClassSubject table |
| Attendance_Date | DATE | NO | - | Date of attendance |
| Status | VARCHAR(10) | YES | - | Present/Absent/Late/Leave |
| Marked_By_Teacher_Id | INT | YES | - | FK to Teachers table |
| **IsActive** | BIT | YES | 1 | ✅ **PascalCase** |
| **IsDeleted** | BIT | YES | 0 | ✅ **PascalCase** - Soft delete flag |
| Created_By | NVARCHAR(100) | YES | - | Audit trail |
| Created_Date | DATETIME | YES | getdate() | Audit trail |
| Modified_By | NVARCHAR(100) | YES | - | Audit trail |
| Modified_Date | DATETIME | YES | - | Audit trail |

**Check Constraints**:
- Status IN ('Present', 'Absent', 'Late', 'Leave')

**Unique Constraints**:
- `UX_Attendance` on (Student_Class_Id, ClassSubject_Id, Attendance_Date) WHERE IsDeleted=0

---

## Naming Conventions Summary

### ✅ PascalCase (Recommended - Modern .NET Standard)
- Classes
- Subjects
- Fees
- Attendance
- ClassSubject

### ⚠️ camelCase (Legacy - Should be migrated)
- Students
- Teachers
- Roles
- StudentClass
- TeacherClass

### Recommendation
For future development, standardize all tables to use **PascalCase** for consistency with modern C# naming conventions.

---

## Soft-Delete Pattern Implementation

All tables include:
- `IsDeleted` or `isDeleted` (BIT, DEFAULT 0)
- `IsActive` or `isActive` (BIT, DEFAULT 1 or 0)
- `Created_By`, `Created_Date` (Audit trail)
- `Modified_By`, `Modified_Date` (Audit trail)

All queries must filter: `WHERE IsDeleted = 0` or `WHERE isDeleted = 0`

---

## Key Relationships

```
Users
  ├── Students (1:1 via User_Id)
  ├── Teachers (1:1 via User_Id)
  └── Roles (reference)

Classes
  ├── ClassSubject (1:N via Class_Id)
  ├── Fees (1:N via Class_Id)
  ├── StudentClass (1:N via Class_Id)
  └── TeacherClass (1:N via Class_Id)

Subjects
  └── ClassSubject (1:N via Subject_Id)

ClassSubject
  ├── TeacherClass (1:N via ClassSubject_Id)
  └── Attendance (1:N via ClassSubject_Id)

StudentClass
  └── Attendance (1:N via Student_Class_Id)

Teachers
  ├── TeacherClass (1:N via Teacher_Id)
  └── Attendance (1:N via Marked_By_Teacher_Id)
```

---

## Error Prevention Checklist

When writing new stored procedures:
- [ ] Use explicit column names (no SELECT *)
- [ ] Include soft-delete filter (WHERE IsDeleted = 0 or isDeleted = 0)
- [ ] Match column naming convention with table
- [ ] Include audit trail columns (Created_By, Created_Date, etc.)
- [ ] Test with data before deployment
- [ ] Document procedure purpose and parameters

---

**Last Updated**: 2025-02-22
**Version**: 2.1 (Fixed Class table reference + Added schema documentation)
