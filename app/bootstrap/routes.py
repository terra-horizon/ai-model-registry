from fastapi import FastAPI, APIRouter

from app.api.health import router as health_router
from app.api.photos import router as photos_router


def configure_routes(app: FastAPI) -> None:
    api_router = APIRouter(prefix="/api")
    api_router.include_router(health_router)
    api_router.include_router(photos_router)
    app.include_router(api_router)