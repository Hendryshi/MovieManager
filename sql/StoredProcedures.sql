CREATE PROCEDURE ResetDataBase
	@idMovie INT = 0
AS
BEGIN
	delete from J_Actor
	delete from J_Category
	delete from J_Company
	delete from J_Director
	delete from J_Publisher
	delete from J_MovieRelation
	delete from J_MovieMagnet
	delete from J_MovieHistory
	delete from J_Movie
	
	DBCC CHECKIDENT ('J_Actor', RESEED, 0)
	DBCC CHECKIDENT ('J_Category', RESEED, 0)
	DBCC CHECKIDENT ('J_Company', RESEED, 0)
	DBCC CHECKIDENT ('J_Director', RESEED, 0)
	DBCC CHECKIDENT ('J_Publisher', RESEED, 0)
	DBCC CHECKIDENT ('J_MovieMagnet', RESEED, 0)
	DBCC CHECKIDENT ('J_MovieHistory', RESEED, 0)
	DBCC CHECKIDENT ('J_Movie', RESEED, 0)
END