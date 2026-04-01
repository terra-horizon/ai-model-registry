from contextlib import asynccontextmanager

from fastapi import FastAPI

from bootstrap.routes import configure_routes
from core.config import get_settings
from core.container import Container


@asynccontextmanager
async def lifespan(app: FastAPI):
    settings = get_settings()
    print(f"Starting {settings.app_name} in {settings.app_env} mode...")
    yield
    print(f"Shutting down {settings.app_name}...")


def create_app() -> FastAPI:
    settings = get_settings()
    container = Container()
    container.config.from_dict(
        {
            "app_name": settings.app_name,
            "app_version": settings.app_version,
            "app_env": settings.app_env,
            "debug": settings.debug,
        }
    )
    app = FastAPI(
        title=settings.app_name,
        version=settings.app_version,
        debug=settings.debug,
        lifespan=lifespan,
    )
    app.container = container
    configure_routes(app)

    return app