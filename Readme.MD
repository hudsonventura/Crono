# Crono
This app will help you schedule container restarts in the same way as crontab, but inside the docker-compose file, together your projects.
<br>
<br>
### About
<br>
You can inform a time (like crontab) to restart like this:  
<img src="images/example.png" alt="Example">
<br>
<br>
<br>
It uses less than 1 MB of RAM with 4 containers.  
<img src="images/memory_usage.png" alt="Memory Usage">


## Requirements
A distro Linux, docker, docker-compose, some knowledge about it and Crontab.
<br>
<br>
## Create your docker-compose.yml file.
```yaml
version: '3'
services:

  service1:
    image: "postgres"
    restart: always
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=myStr0ngP4ssw0rd
    labels:
        crono: "* * * * *"

  service2:
    image: "postgres"
    restart: always
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=myStr0ngP4ssw0rd
    labels:
        crono: "@hourly"

  service3:
    image: "postgres"
    restart: always
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=myStr0ngP4ssw0rd
    labels:
        crono: "5 0 * 8 *"

  service4:
    image: "postgres"
    restart: always
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=myStr0ngP4ssw0rd
    labels:
        crono: "50-59 * * * 1-5"
```
See that inside each service I put a `label` called `crono`. This will call to Crono that we want to restart it (or start if it was down). The value of this will be the same as Crontab.<br>
For help you, you can see `crontab.guru` at https://crontab.guru<br>
You can infform some keys like @yearly, @annually, @monthly, @weekly, @daily or @hourly.
<br>
<br>
## Put the Crono service

Functional example 1:
```yaml
  crono:
    image: hudsonventura/crono:latest
    restart: always
    volumes:
        - "./docker-compose.yml:/app/docker-compose.yml" 
        - "/var/run/docker.sock:/var/run/docker.sock:ro"
        #- "./logs/:/app/logs/" #Optional
    
    environment: 
        #- TZ=America/Sao_Paulo 
        - WAIT=10
```
The volume `- "./docker-compose.yml:/app/docker-compose.yml"`, you have to point your docker-compose file. Crono will read this file to work.
<br>
The volume `- "/var/run/docker.sock:/var/run/docker.sock:ro"`, will used from Crono to call Docker Api.
<br>
The volume `- "./logs/:/app/logs/"`, will used to persist logs, if you need.
<br>
<br>
The environment `#- TZ=America/Sao_Paulo`, you can identify a timezone. If you ignore, it'll assume Etc/UTC. You can see a complete list in [https://en.wikipedia.org/wiki/List_of_tz_database_time_zones](https://en.wikipedia.org/wiki/List_of_tz_database_time_zones)
<br>
<br>
The environment `WAIT=10` It'll wait how many seconds you set before read and start the operations on you containers.<br>
<br>
<br>

## If you want to build your Docker image ...

Just:

```bash
sudo docker compose up
```