# Crono
This repo will help you to schedule to restart containers at the same way  as crontab, but inside the docker-compose file.
<br>
<br>
## Requirements
A distro Linux, docker, docker-compose, some knowledg about it and Crontab.
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
See that inside each service I put a label called 'crono'. This will call to Crono that we want to restart it. The value of this will be the same like Crontab.<br>
For help you, you can see crontab.guru at https://crontab.guru<br>
You can infform some keys like @yearly, @annually, @monthly, @weekly, @daily or @hourly.
<br>
<br>
## Put the Crono service
```yaml
  crono:
    image: hudsonventura/crono:latest
    restart: always
    volumes:
        - "./docker-compose.yml:/app/docker-compose.yml" # point you docker-compose file
        - "/var/run/docker.sock:/var/run/docker.sock:ro"
    
    environment: 
        - TIMEZONE=America/Sao_Paulo
        - WAIT=3
```
The first one volume, you have to link your docker-compose file. Crono will read this file to work.
<br>
<br>
The second one volume, will used from Crono to call Docker Api.
<br>
<br>
The first one environment, you can identify a timezone. In my tests the default timezone inside container was +0UTC, so I created this.
<br>
<br>
The second one environment is the time that Crono will wait until all containers be up.<br>
You also can use the docker-compose params called links.
<br>
<br>
The volumes params are mandatories, but the environments are optional.
<br>
<br>
<br>
The complete example file is in the directory examples.
<br>
<br>

## If you want to compile you image ...
<br>

Change docker compose file to get local image:
```yaml
crono:
    image: crono  #HERE
    restart: always
    volumes:
        - "./docker-compose.yml:/app/docker-compose.yml"
        - "/var/run/docker.sock:/var/run/docker.sock:ro"
    
    environment: 
        - TIMEZONE=America/Sao_Paulo
        - WAIT=3
```

<br>
On example directory, run it to build:

``` bash
sudo docker compose down && \
cd .. && \
sudo docker build -t crono -f ./app/Dockerfile_x64_x86/Dockerfile ./app/ && \
cd example/ && \
sudo docker compose up
```
