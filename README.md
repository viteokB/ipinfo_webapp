# ipinfo_webapp

## Описание

Веб-приложение, созданное мной для того, чтобы попрактиковать получение связанной информации с IP клиента и его идентификации по стране и т.п. данным.

## [Web API](https://github.com/viteokB/ipinfo_webapp/tree/main/backend)

### Смысл API

Предоставлять по определенному URL информацию об IP-адресе клиента, отправившего запрос к нему. Также можно получить информацию по любому введеному IP.
И выбрать сервис из которого получить информацию об IP.

### Где берется информация связанная с IP

1) [IPInfo](https://ipinfo.io/)

Возможность бесплатного плана: 50_000 запросов в месяц.
Ну и само сабой достаточно ограниченная инфа.

2) [ipbase.com](https://ipbase.com/)

Возможность бесплатного плана: всего 150 запросов.
Ну и само сабой достаточно ограниченная инфа.

3) [AbuseIPDB](https://www.abuseipdb.com/)

| Endpoint         | Usage / Daily Limit | Utilization Rate |
|------------------|---------------------|------------------|
| blacklist        | 0 / 5               | 0%               |
| bulk-report      | 0 / 5               | 0%               |
| check            | 43 / 1,000          | 4.3%             |
| check-block      | 0 / 100             | 0%               |
| clear-address    | 0 / 5               | 0%               |
| report           | 0 / 1,000           | 0%               |
| reports          | 0 / 100             | 0%               |

### Таблица с доступными URL-ресурсами для работы с IP-адресами и их описанием

| Метод | URL-путь | Описание | Параметры |
|-------|----------|----------|-----------|
| GET | `/api/ipinfo/{ip}` | Возвращает базовую информацию об IP (ASN, страна, континент) | `ip` (в пути), `api-version` (в заголовке) |
| GET | `/api/freegeoip/{ip}` | Возвращает географическую информацию об IP (локация, город, регион) | `ip` (в пути), `api-version` (в заголовке) |
| GET | `/api/abuseipdb/{ip}` | Возвращает данные о репутации IP из AbuseIPDB | `ip` (в пути), `maxAgeInDays` (в query, по умолчанию 90), `api-version` (в заголовке) |
| GET | `/api/ipinfo/my` | Возвращает базовую информацию о вашем IP | `api-version` (в заголовке) |
| GET | `/api/freegeoip/my` | Возвращает географическую информацию о вашем IP | `api-version` (в заголовке) |
| GET | `/api/abuseipdb/my` | Возвращает данные о репутации вашего IP из AbuseIPDB | `maxAgeInDays` (в query, по умолчанию 90), `api-version` (в заголовке) |
| GET | `/api/full/{ip}` | Возвращает полную информацию об IP (объединяет все источники) | `ip` (в пути), `api-version` (в заголовке) |
| GET | `/api/full/my` | Возвращает полную информацию о вашем IP (объединяет все источники) | `api-version` (в заголовке) |
| OPTIONS | `/api` | Метод для проверки доступности API | `api-version` (в заголовке) |

### <span style="color:red">**ВАЖНО:** Необходимо создать appsettings.json в директории IPInfoWebApi перед запуском!</span>

Пример минимальной конфигурации:
```json
{
  "TokensPool" : {
    "IpInfoToken": "ТОКЕН",
    "FreeGeoIpToken": "ТОКЕН",
    "AbuseIpDbToken": "ТОКЕН"
  },
  "AllowedHosts": "*"
}
```

### [Frontend](https://github.com/viteokB/ipinfo_webapp/tree/main/frontend)

На счет фронта сказать ничего не могу, я его просто навайбкодил, для того чтобы можно было в более красивой форме видеть данные.
Могу сказать что тут используется пара библиотек с уязвимостями.

### Docker Compose 

В "локальной" версии здесь в небольшом отличии от версии развернутой на https://vitiok-projects.ru/ 

Мы создаем:

1) Сервисы:
   1) Nginx (обратный прокси)
    - Является единой точкой входа для приложения
    - Использует облегченный образ nginx на alpine linux stable версии
    - Пробрасывает 80 порт хоста на 80 контейнера
    - Монтирует файл ./[nginx.conf](https://github.com/viteokB/ipinfo_webapp/blob/main/nginx.conf) с хоста в контейнер в /etc/nginx/nginx.conf
    - Зависит(Запускает) nginx только после готовности сервисов frontend и backend
    - network - подключает к сети network для взаимодействия с контейнерами.
   2) Backend (веб апи)
    - объявляем имя контейнера в container_name
    - объявляем имя используемого образа
    - в build объявляем параметры процесса сборки, прописываем контекст сборки и название [dockerfile](https://github.com/viteokB/ipinfo_webapp/blob/main/backend/Dockerfile)
    - network - подключает к сети network для взаимодействия с контейнерами.
   3) Frontend по описанию настроек аналогично backend различие в расположении [dockerfile](https://github.com/viteokB/ipinfo_webapp/blob/main/frontend/Dockerfile)
3) bridge-сеть networkName для взаимодействия контейнеров
4) Проект ipinfo_application

```yaml
services:
  nginx:
    image: nginx:stable-alpine
    ports:
     - "80:80"
    volumes:
     - "./nginx.conf:/etc/nginx/nginx.conf"
    depends_on:
      - frontend
      - backend
    networks:
      - networkName

  backend:
    container_name: ipinfo_webapi
    image: ${DOCKER_REGISTRY-}ipinfo_webapi
    build:
      context: ./Backend
      dockerfile: Dockerfile
    networks:
      - networkName
  
  frontend:
    container_name: ipinfo_frontend
    image: ipinfo_frontend
    build:
      context: ./frontend
      dockerfile: Dockerfile
    networks:
      - networkName

name: ipinfo_application
networks:
  networkName:
    driver: bridge
```

В версии развернутой сервере, я значительно усложняю nginx.conf, т.к. хочу получить опыт работы с SSL сертификатами и чтобы соединение было по https.

## Описание "Dockerfileов"

1) [backend](https://github.com/viteokB/ipinfo_webapp/blob/main/backend/Dockerfile)

```yaml
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IPInfoWebApi/IPInfoWebApi.csproj", "IPInfoWebApi/"]
RUN dotnet restore "IPInfoWebApi/IPInfoWebApi.csproj"
COPY . .
WORKDIR "/src/IPInfoWebApi"
RUN dotnet build "IPInfoWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IPInfoWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IPInfoWebApi.dll"]
```

Краткий разбор Dockerfile для .NET 8.0
1) Многоступенчатая сборка
   - base – легковесный образ (aspnet:8.0) для runtime.
   - build – образ с SDK (sdk:8.0) для компиляции.
   - publish – публикация приложения.
   - final – итоговый образ (только нужные файлы).

2) Порт 8080.

3) COPY .csproj + dotnet restore – кэширование NuGet.

4) dotnet publish – минимизация финального образа.

5) Запуск ENTRYPOINT ["dotnet", "IPInfoWebApi.dll"] – старт приложения.

2) [frontend](https://github.com/viteokB/ipinfo_webapp/blob/main/frontend/Dockerfile)

```yaml
FROM node:alpine as build

# Копируем package.json и package-lock.json
COPY package*.json ./

# Устанавливаем зависимости
RUN npm install

# Копируем все файлы проекта
COPY . .

# Собираем проект
RUN npm run build

FROM nginx:stable-alpine

# Копируем собранные файлы во второй этап
COPY --from=build /build /usr/share/nginx/html

# Копируем конфигурацию nginx
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 7000

CMD ["nginx", "-g", "daemon off;"]
```

Этот Dockerfile использует многоступенчатую сборку для создания оптимизированного образа фронтенд-приложения:

1) Сборка (build stage)
  - Берется образ node:alpine (легковесный Node.js).
  - Копируются package.json и package-lock.json, устанавливаются зависимости (npm install).
  - Копируются все файлы проекта и запускается сборка (npm run build).

2) Production-образ
  - Берется легковесный nginx:stable-alpine.
  - Копируются только собранные файлы из папки /build (результат сборки) в Nginx-папку /usr/share/nginx/html.
  - Заменяется конфиг Nginx (nginx.conf).
  - Открывается порт 7000 и запускается Nginx в foreground-режиме (daemon off).

Содержит только Nginx и статику (без Node.js и исходников).
