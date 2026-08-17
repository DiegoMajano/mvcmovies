# [Nombre del Proyecto] - Despliegue en la Nube

## Nota sobre la plataforma utilizada

Este proyecto fue desplegado usando **Render** en lugar de Microsoft Azure, debido a no
contar con una cuenta de Azure for Students disponible.
Render ofrece un servicio equivalente:

| Concepto (Azure) | Equivalente usado (Render) |
|---|---|
| Azure App Service | Render Web Service (despliegue vía Docker) |
| Azure SQL Database | Postgress Render |
| Publicación desde Visual Studio | Despliegue automático vía GitHub + render.yaml (Blueprint) |

## Tecnologías

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- Postgress
- Docker / Docker Compose
- Render (Blueprint deployment)

## Cómo correr localmente

```bash
docker-compose up --build
```

Luego abrir: http://localhost:8080

## Cómo se desplegó en Render

1. Repositorio conectado a Render vía Blueprint (`render.yaml`)
2. Servicio `movies-RM220481`: build y deploy desde el `Dockerfile`
3. Servicio `mvcpeliculas-db-rm220481`: Render Database Postgress
4. Migraciones de EF Core se aplican automáticamente al iniciar la app (`Database.Migrate()`)

## URL pública

https://mvcmovies.onrender.com

## Limitaciones conocidas

El plan gratuito de Render usa almacenamiento efímero (sin disco persistente),
por lo que los datos de la base se reinician si el contenedor se reinicia.