from contextlib import asynccontextmanager

from fastapi import FastAPI

from bootstrap.routes import configure_routes
from core.config import get_settings


@asynccontextmanager
async def lifespan(app: FastAPI):
    settings = get_settings()
    print(f"Starting {settings.app_name} in {settings.app_env} mode...")
    yield
    print(f"Shutting down {settings.app_name}...")


def create_app() -> FastAPI:
    settings = get_settings()

    app = FastAPI(
        title=settings.app_name,
        version=settings.app_version,
        debug=settings.debug,
        lifespan=lifespan,
    )

    configure_routes(app)

    return app