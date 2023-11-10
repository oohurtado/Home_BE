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