GO
CREATE VIEW vMovieFavLevel
AS
SELECT DISTINCT m.idMovie, m.number, CASE m.favlevel WHEN 0 THEN 0 ELSE (SELECT MAX(v) FROM (VALUES (m.favlevel), (a.favlevel), (c.favlevel)) AS VALUE(v)) END idFavLevel
FROM J_Movie m
LEFT JOIN J_MovieRelation ma ON m.idMovie = ma.idMovie AND ma.idTyRole = 5
LEFT JOIN J_MovieRelation mc ON m.idMovie = mc.idMovie AND mc.idTyRole = 2
LEFT JOIN J_Actor a ON a.idActor = ma.idRelation
LEFT JOIN J_Company c ON c.idCompany = mc.idRelation
GO

GO
CREATE VIEW vMovieMagnetInfo
AS
SELECT idMovie, movieNumber, COUNT(*) nbMag, MAX(CAST(isHD AS INT)) hasHD, max(cast(hasSub AS INT)) hasSub
FROM J_MovieMagnet
WHERE idStatus NOT IN (0,4) -- 0 => Dead, 4 => InError
GROUP BY idMovie, movieNumber
GO

GO
-- Load Magnets To Download
--SELECT idMovie FROM vMovieInfo
--WHERE (idFavLevel > 0 AND nbMag = 0) OR (idFavLevel > 2 AND hasSub = 0)
--AND DATEADD(MONTH, 2, dtRelease) > GETDATE()
--ORDER BY isUrgent DESC, dtRelease DESC, hasSub, hasHD

-- Load Movies To Download
--SELECT idMovie FROM vMovieInfo
--WHERE idMovie NOT IN (SELECT idMovie FROM J_MovieMagnet WHERE idStatus IN (2,4))
--AND (
--	(idStMovie = 3 AND idFavLevel = 3 AND hasSub = 1 AND hasHD = 1)  -- Movie Downloaded has sub torrent to be downloaded
--	OR (idStMovie = 2 AND idFavLevel > 1 AND hasHD = 1) -- Movie has torrent that need to be downloaded
--)
--AND DATEADD(YEAR, 1, dtRelease) > GETDATE()
--ORDER BY isUrgent DESC, dtRelease DESC, idStMovie DESC
CREATE VIEW vMovieInfo
AS
SELECT m.idMovie, m.number, dmf.descLevel descFavLevel, (nbWant+nbWatched+nbOwned) point, duration
		, m.dtRelease, dms.descStatus descStMovie, isUrgent
		, mm.idMovieMag mainMagnetId, mm.hash mainMagnetHash
		, ISNULL(nbMag, 0) nbMag, ISNULL(vmmi.hasHD, 0) hasHD, ISNULL(vmmi.hasSub, 0) hasSub
		, dmf.idLevel idFavLevel, dms.idStatus idStMovie
FROM J_Movie m
JOIN vMovieFavLevel vmf on m.idMovie = vmf.idMovie
JOIN D_MovieFavLevel dmf ON dmf.idLevel = vmf.idFavlevel
JOIN D_MovieStatus dms ON dms.idStatus = m.idStatus
LEFT JOIN vMovieMagnetInfo vmmi on vmmi.idMovie = m.idMovie
LEFT JOIN J_MovieMagnet mm ON m.idMovie = mm.idMovie AND mm.idStatus = 3
WHERE m.category NOT LIKE '%VR%' AND ISNULL(LEN(m.actor) - LEN(REPLACE(m.actor, ',', '')), 0) < 4
GO