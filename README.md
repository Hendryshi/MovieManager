# MovieManager

## Context

A .Net Core application which will collect the new released movies from the website to local data sources, and download certain movies to NAS driver.
Two jobs was created to perfom the daily tasks.


## Technical used
- .Net Core, Sql Server, Hangfire jobs, Dapper


## Daily background jobs

- Job scrape (Daily mode)

  - Scrape the new released movies information from the site Javlibrary and insert them to the DB
  - Scrape the torrent files for movies (retrieved by a SQL view ) from the site JavBus and/or Sukebei.Nyaa

- Job Download (Daily mode)

  - This job will be launched continually (every 30 mins) to download the movies and put them to the correct place in NAS driver

  - Monitor the current downloading torrent in application qbittorrent

  - If download finished, create the folder in NAS driver and move the movie to the folder


## Other features in the future

- [ ] Create a report service every day/week
- [ ] Add a visualization tool to monitor the database changes
- [ ] Add a synchronisation tool to update the local driver file & database file
- [ ] Improve the movie history issue (too many)


