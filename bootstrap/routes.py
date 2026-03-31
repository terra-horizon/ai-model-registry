from fastapi import FastAPI

from api.health import router as health_router
from api.photos import router as photos_router


def configure_routes(app: FastAPI) -> None:
    app.include_router(health_router)
    app.include_router(photos_router)