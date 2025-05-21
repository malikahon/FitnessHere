
-- index for frequent search columns 
CREATE NONCLUSTERED INDEX index_Members_DateOfBirth ON Members(DateOfBirth) INCLUDE (FirstName, LastName, PhoneNumber);
-- covering index for filtering by IsDisabled and IsMale
CREATE NONCLUSTERED INDEX index_Members_Status ON Members (IsDisabled, IsMale);
-- index for trainers table to speed up lookups by email
CREATE UNIQUE NONCLUSTERED INDEX index_Trainers_Email ON Trainers(Email);
-- index on Experience Years to speed up range queries
CREATE NONCLUSTERED INDEX index_Trainers_Experience ON Trainers(ExperienceYears) INCLUDE (FirstName, LastName, PhoneNumber, Salary);
-- index on members first name last name 
CREATE INDEX index_Members_FirstName_LastName_RegistrationDate ON Members (FirstName, LastName, RegistrationDate);
-- index on memberID to speed up CRUD
CREATE INDEX index_Members_MemberID ON Members (MemberID);
-- index on className for filtering
CREATE INDEX index_Classes_ClassName ON Classes (ClassName);


-- create logins for users (authentication at server level)
CREATE LOGIN AdminUser WITH PASSWORD = 'StrongPassword123!';
CREATE LOGIN RegularUser WITH PASSWORD = 'UserPassword!';
CREATE LOGIN ManagerUser WITH PASSWORD = 'ManagerPassword!';
CREATE LOGIN CashierUser WITH PASSWORD = 'CashierPass!';
CREATE LOGIN HRUser WITH PASSWORD = 'HRpassword!';
CREATE LOGIN DirectorUser WITH PASSWORD = 'DirectorPass!';

-- map logins to database users (inside the database)
USE [C:\USERS\NKUCH\ONEDRIVE\DESKTOP\FITNESSHERE\APPDATA\FITNESSDATABASE.MDF];
CREATE USER Administrator FOR LOGIN AdminUser;
CREATE USER [User] FOR LOGIN RegularUser;
CREATE USER Manager FOR LOGIN ManagerUser;
CREATE USER Cashier FOR LOGIN CashierUser;
CREATE USER HR FOR LOGIN HRUser;
CREATE USER Director FOR LOGIN DirectorUser;


-- administrator Role (front office)
CREATE ROLE AdministratorRole;
GRANT SELECT, INSERT ON Members TO AdministratorRole;  -- Can view & create users
GRANT SELECT, INSERT ON MemberClasses TO AdministratorRole; -- Can view & create new classes
GRANT SELECT ON Trainers TO AdministratorRole; -- Can only view trainers
ALTER ROLE AdministratorRole ADD MEMBER Administrator;

-- manager role (unrestricted)
CREATE ROLE ManagerRole;
GRANT CONTROL ON DATABASE:: [C:\USERS\NKUCH\ONEDRIVE\DESKTOP\FITNESSHERE\APPDATA\FITNESSDATABASE.MDF] TO ManagerRole; -- Full permissions
ALTER ROLE ManagerRole ADD MEMBER Manager;

-- cashier role (can only create transactions)
CREATE ROLE CashierRole;
GRANT INSERT ON Transactions TO CashierRole;
ALTER ROLE CashierRole ADD MEMBER Cashier;

-- HR role (create and view trainers)
CREATE ROLE HRRole;
GRANT SELECT, INSERT ON Trainers TO HRRole;
ALTER ROLE HRRole ADD MEMBER HR;

-- director role (unrestricted)
CREATE ROLE DirectorRole;
GRANT CONTROL ON DATABASE::[C:\USERS\NKUCH\ONEDRIVE\DESKTOP\FITNESSHERE\APPDATA\FITNESSDATABASE.MDF] TO DirectorRole;
ALTER ROLE DirectorRole ADD MEMBER Director;
  



 alter role AdministratorRole drop member Administrator;
 alter role UserRole drop member [User];
 alter role ManagerRole drop member Manager;
 alter role CashierRole drop member Cashier;
 alter role HRRole drop member HR;
 alter role DirectorRole drop member Director;

 drop role AdminstratorRole;
 drop role UserRole;
 drop role ManagerRole;
 drop role CashierRole;
 drop role HRRole;
 drop role DirectorRole;

 drop user Administrator;
 drop user [User];
 drop user Manager;
 drop user Cashier;
 drop user HR;
 drop user Director;

 drop login AdminUser;
 drop login RegularUser;
 drop login ManagerUser;
 drop login CashierUser;
 drop login HRUser;
 drop login DirectorUser;
