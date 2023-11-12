-- ctrl + shift + r = actualiza intellisense

---- Programmability > Types > User-Defined Table Types
--CREATE TYPE IdList AS TABLE (Id int);





---- Programmability > Stored Procedures
--CREATE PROCEDURE [dbo].[sp_GetByIdList]
--	@p_ids IdList READONLY
--AS
--BEGIN
--	SET NOCOUNT ON
--	SELECT p.PersonId, p.FirstName, p.LastName FROM People p WHERE p.PersonId IN (SELECT Id FROM @p_ids)
--END





---- Programmability > Stored Procedures
--CREATE PROCEDURE [dbo].[sp_InsertPerson]
--	@p_firstname NVARCHAR(25),
--	@p_lastname NVARCHAR(25),
--	@out_id INT OUTPUT
--AS
--BEGIN
--	SET NOCOUNT ON
--	INSERT INTO People(FirstName, LastName)
--	VALUES(@p_firstname,  @p_lastname);

--	SELECT @out_id = SCOPE_IDENTITY();	
--END





---- Programmability > Stored Procedures
--CREATE PROCEDURE [dbo].[sp_GetById]
--	@p_id INT
--AS
--BEGIN
--	SELECT p.PersonId, p.FirstName, p.LastName FROM People p
--	WHERE p.PersonId = @p_id
--END





---- Programmability > Stored Procedures
--CREATE PROCEDURE [dbo].[sp_GetIds]
--AS
--BEGIN
--	SELECT p.PersonId FROM People p
--END





---- Programmability > Stored Procedures
--CREATE PROCEDURE [dbo].[sp_GetPeople]
--AS
--BEGIN
--	SELECT p.PersonId, p.FirstName, p.LastName FROM People p
--END