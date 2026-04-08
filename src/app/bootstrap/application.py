from contextlib import asynccontextmanager

import structlog
from fastapi import FastAPI

from app.bootstrap.logging import configure_logging
from app.bootstrap.routes import configure_routes
from app.core.config import get_settings
from app.core.container import Container
from app.middleware.logging import LoggingMiddleware


@asynccontextmanager
async def lifespan(app: FastAPI):
    settings = get_settings()
    print(f"Starting {settings.app_name}...")
    yield
    print(f"Shutting down {settings.app_name}...")


def create_app() -> FastAPI:
    settings = get_settings()
    container = Container()
    container.config.from_dict(
        {
            "app_name": settings.app_name,
            "app_version": settings.app_version,
            "debug": settings.debug,
        }
    )
    configure_logging(
        json_logs=settings.debug,
        log_level=settings.verbosity,
    )
    app = FastAPI(
        title=settings.app_name,
        version=settings.app_version,
        debug=settings.debug,
        lifespan=lifespan,
    )
    app.add_middleware(LoggingMiddleware)
    app.container = container
    configure_routes(app)

    return app