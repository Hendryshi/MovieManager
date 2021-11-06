CREATE TABLE J_Movie
(	
	idMovie int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	number NVARCHAR(20) NULL UNIQUE,
	title NVARCHAR(300) NULL,
	company NVARCHAR(1000) NULL,
	director NVARCHAR(1000) NULL,
	publisher NVARCHAR(1000) NULL,
	category NVARCHAR(1000) NULL,
	actor NVARCHAR(4000) NULL,
	dtRelease DATETIME NULL,
	duration INT NULL,
	nbWant INT NULL,
	nbWatched INT NULL,
	nbOwned INT NULL,
	thumbnailUrl NVARCHAR(1000) NULL,
	coverUrl NVARCHAR(1000) NULL,
	favLevel SMALLINT NULL DEFAULT 0,
	isUrgent BIT NOT NULL DEFAULT 0,
	idStatus SMALLINT NOT NULL,
	url NVARCHAR(500) NULL,
	dtUpdate DATETIME NULL,
	CONSTRAINT FK_J_Movie_IdStatus FOREIGN KEY (idStatus) REFERENCES D_MovieStatus(idStatus)
)

CREATE TABLE J_MovieRelation
(
	idMovie INT NOT NULL,
	idTyRole SMALLINT NOT NULL,
	idRelation INT NOT NULL,
	CONSTRAINT FK_MovieRelation_IdMoive FOREIGN KEY (idMovie) REFERENCES J_Movie(idMovie),
	CONSTRAINT FK_MovieRelation_IdTyRole FOREIGN KEY (idTyRole) REFERENCES D_MovieRoleType(idTyRole),
	CONSTRAINT PK_MovieRelation PRIMARY KEY (idMovie,idTyRole, idRelation)
)

CREATE TABLE J_Category
(	
	idCategory INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	name NVARCHAR(100) NULL,
	url NVARCHAR(500) NULL,
	description NVARCHAR(500) NULL,
	favLevel SMALLINT NULL DEFAULT(0),
	dtUpdate DATETIME NULL,
	CONSTRAINT FK_J_Category_FavLevel FOREIGN KEY (favLevel) REFERENCES D_MovieFavLevel(idLevel)
)

CREATE TABLE J_Company
(	
	idCompany INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	name NVARCHAR(100) NULL,
	url NVARCHAR(500) NULL,
	description NVARCHAR(500) NULL,
	favLevel SMALLINT NULL DEFAULT(0),
	dtUpdate DATETIME NULL,
	CONSTRAINT FK_J_Company_FavLevel FOREIGN KEY (favLevel) REFERENCES D_MovieFavLevel(idLevel)
)

CREATE TABLE J_Director
(	
	idDirector INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	name NVARCHAR(100) NULL,
	url NVARCHAR(500) NULL,
	description NVARCHAR(500) NULL,
	favLevel SMALLINT NULL DEFAULT(0),
	dtUpdate DATETIME NULL,
	CONSTRAINT FK_J_Director_FavLevel FOREIGN KEY (favLevel) REFERENCES D_MovieFavLevel(idLevel)
)

CREATE TABLE J_Publisher
(	
	idPublisher INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	name NVARCHAR(100) NULL,
	url NVARCHAR(500) NULL,
	description NVARCHAR(500) NULL,
	favLevel SMALLINT NULL DEFAULT(0),
	dtUpdate DATETIME NULL,
	CONSTRAINT FK_J_Publisher_FavLevel FOREIGN KEY (favLevel) REFERENCES D_MovieFavLevel(idLevel)
)

CREATE TABLE J_Actor
(	
	idActor INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	name NVARCHAR(100) NULL,
	url NVARCHAR(500) NULL,
	description NVARCHAR(500) NULL,
	favLevel SMALLINT NULL DEFAULT(0),
	dtUpdate DATETIME NULL,
	CONSTRAINT FK_J_Actor_FavLevel FOREIGN KEY (favLevel) REFERENCES D_MovieFavLevel(idLevel)
)

CREATE TABLE J_MovieMagnet(
	idMovieMag INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	idMovie INT NOT NULL,
	movieNumber NVARCHAR(20) NOT NULL,
	magName NVARCHAR(500) NOT NULL,
	magnetUrl NVARCHAR(4000) NOT NULL,
	hash  NVARCHAR(1000) NOT NULL UNIQUE,
	size DECIMAL(10,2) NULL,
	dtMagnet DATETIME NULL,
	isHD BIT NOT NULL DEFAULT 0,
	hasSub BIT NOT NULL DEFAULT 0,
	idMagSource SMALLINT NOT NULL,
	dtStart DATETIME NULL,
	dtFinish DATETIME NULL,
	savePath NVARCHAR(500) NULL,
	idStatus SMALLINT NOT NULL,
	dtUpdate DATETIME NOT NULL,
	CONSTRAINT FK_J_MovieMagnet_IdMoive FOREIGN KEY (idMovie) REFERENCES J_Movie(idMovie),
	CONSTRAINT FK_MovieMagnet_IdMagSource FOREIGN KEY (idMagSource) REFERENCES D_MagnetSource(idMagSource),
	CONSTRAINT FK_J_MovieMagnet_IdStatus FOREIGN KEY (idStatus) REFERENCES D_MagnetStatus(idStatus)
)

CREATE TABLE J_MovieHistory(
	idMovieHistory INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	idMovie INT NOT NULL,
	descHistory NVARCHAR(4000) NULL,
	dtCreation DATETIME NOT NULL CONSTRAINT DF_J_MovieHistory_dtCreation  DEFAULT GETDATE(),
	isActive BIT NOT NULL DEFAULT 1,
	CONSTRAINT FK_J_MovieHistory_IdMoive FOREIGN KEY (idMovie) REFERENCES J_Movie(idMovie),
)

CREATE TABLE D_MovieRoleType
(
	idTyRole SMALLINT NOT NULL PRIMARY KEY,
	descTyRole NVARCHAR(200) NOT NULL
)
GO
INSERT INTO D_MovieRoleType(idTyRole, descTyRole)
VALUES(1, 'Category'), (2, 'Company'), (3, 'Director'), (4, 'Publisher'), (5, 'Star')


CREATE TABLE D_MagnetSource
(
	idMagSource SMALLINT NOT NULL PRIMARY KEY,
	descMagSource NVARCHAR(200) NOT NULL
)
GO
INSERT INTO D_MagnetSource(idMagSource, descMagSource)
VALUES(1, 'https://www.javbus.com/{0}'), (2, 'https://sukebei.nyaa.si/?f=0&c=0_0&q={0}')


CREATE TABLE D_MovieStatus
(
	idStatus SMALLINT NOT NULL PRIMARY KEY,
	descStatus NVARCHAR(200) NOT NULL
)
GO
INSERT INTO D_MovieStatus(idStatus, descStatus)
VALUES(0, 'NotScanned'), (1, 'Scanned'), (2, 'HasTorrent'), (3, 'Downloaded'), (4, 'Finished'), (5, 'InError')


CREATE TABLE D_MagnetStatus
(
	idStatus SMALLINT NOT NULL PRIMARY KEY,
	descStatus NVARCHAR(200) NOT NULL
)
GO
INSERT INTO D_MagnetStatus(idStatus, descStatus)
VALUES(0, 'Dead'), (1, 'IsReady'), (2, 'Downloading'), (3, 'Finished'), (4, 'InError'), (5, 'Archived')


CREATE TABLE D_MovieFavLevel
(
	idLevel SMALLINT NOT NULL PRIMARY KEY,
	descLevel NVARCHAR(200) NOT NULL
)
GO
INSERT INTO D_MovieFavLevel(idLevel, descLevel)
VALUES(0, 'NotInterest'), (1, 'DlTorrent'), (2, 'DlMovie'), (3, 'DlChineseSub')