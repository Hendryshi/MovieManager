CREATE TABLE J_ScrapeReport
(
	idReport INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	dtReport DATETIME NOT NULL UNIQUE,
	nbReleased INT NOT NULL,
	nbInterest SMALLINT NOT NULL,
	nbDownload INT NOT NULL,
	isSent BIT NOT NULL DEFAULT 0
)