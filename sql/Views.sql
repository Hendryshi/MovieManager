CREATE VIEW vMovieToDownloadMag
AS
WITH MovieMagnetInfo
AS
(	
	SELECT idMovie, movieNumber, COUNT(*) nbMag, MAX(CAST(isHD AS INT)) hasHD, MAX(CAST(hasSub AS INT)) hasSub
	FROM J_MovieMagnet
	WHERE idStatus <> 0
	GROUP BY idMovie, movieNumber
),
MovieFavLevel(idMovie, idFavlevel)
AS
(
	SELECT DISTINCT m.idMovie, CASE m.favlevel WHEN 0 THEN 0 ELSE (SELECT MAX(v) FROM (VALUES (m.favlevel), (a.favlevel), (c.favlevel)) AS VALUE(v)) END
	FROM J_Movie m
	LEFT JOIN J_MovieRelation ma ON m.idMovie = ma.idMovie AND ma.idTyRole = 5
	LEFT JOIN J_MovieRelation mc ON m.idMovie = mc.idMovie AND mc.idTyRole = 2
	LEFT JOIN J_Actor a ON a.idActor = ma.idRelation
	LEFT JOIN J_Company c ON c.idCompany = mc.idRelation
)
SELECT m.idMovie, m.number, mf.idFavlevel, nbMag
FROM J_Movie m
JOIN MovieFavLevel mf ON m.idMovie = mf.idMovie
LEFT JOIN MovieMagnetInfo mi ON mi.idMovie = m.idMovie
WHERE ((mf.idFavlevel > 0 AND ISNULL(nbMag,0) = 0) OR (mf.idFavlevel > 2 AND ISNULL(hasSub,0) = 0)) AND DATEADD(MONTH, 2, m.dtRelease) > GETDATE()
