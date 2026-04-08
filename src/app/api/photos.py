from typing import Annotated

import structlog
from dependency_injector.wiring import inject, Provide
from fastapi import APIRouter, Depends, File, UploadFile, Security

from app.core.container import Container
from app.schemas.photos import PhotoAnalysisResponse, PhotoBase64Request
from app.security.current_user import get_current_user
from app.security.models import CurrentUser
from app.services.photo_service import PhotoService

router = APIRouter(prefix="/photos", tags=["Photos"], )


@router.post(path="/infer", response_model=PhotoAnalysisResponse, summary="Receive a photo for processing", )
@inject
async def inspect_photo(
        request: PhotoBase64Request,
        current_user: Annotated[CurrentUser, Security(get_current_user)],
        photo_service: Annotated[PhotoService, Depends(Provide[Container.photo_service])],
) -> PhotoAnalysisResponse:
    logger = structlog.get_logger()
    logger.info("Inferring", request)
    return await photo_service.inspect_photo(request)
