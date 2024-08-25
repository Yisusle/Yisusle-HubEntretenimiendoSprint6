
FROM node:18 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build:ssr

FROM node:18-alpine
WORKDIR /app
COPY --from=build /app/dist /app/dist
COPY --from=build /app/package*.json ./

RUN npm install --only=production

EXPOSE 4000
CMD ["npm", "run", "serve:ssr"]

