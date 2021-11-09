-- See what actor need to be added to favorite
select c.name, df.descLevel, avg(m.nbOwned + m.nbWant + m.nbWatched) from J_Movie m
join J_MovieRelation mc on m.idMovie = mc.idMovie and mc.idTyRole = 5
join J_Actor c on mc.idRelation = c.idActor
join D_MovieFavLevel df on c.favLevel = df.idLevel
where c.favLevel < 2
group by c.name, df.descLevel
order by avg(m.nbOwned + m.nbWant + m.nbWatched) desc