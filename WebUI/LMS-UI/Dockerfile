FROM node:12.16.3-alpine as node
#FROM node:12-alpine as node 
RUN apk update \
&& apk add --virtual build-dependencies \
        build-base \
        gcc \
        wget \
        git \
&& apk add \
bash

 

WORKDIR /usr/src/app
COPY package*.json ./
RUN npm install
COPY . .
RUN node --max_old_space_size=8100 ./node_modules/@angular/cli/bin/ng build  --configuration=development --baseHref=/lms-ui/
# RUN node --max_old_space_size=8100 ./node_modules/@angular/cli/bin/ng build --prod

 

# Stage 2
FROM nginx:1.17-alpine
COPY --from=node /usr/src/app/dist/lms-ui /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
CMD ["nginx", "-g", "daemon off;"]
