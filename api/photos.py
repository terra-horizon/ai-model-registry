from typing import Annotated

from dependency_injector.wiring import inject, Provide
from fastapi import APIRouter, Depends, File, UploadFile

from core.container import Container
from schemas.photos import PhotoAnalysisResponse
from services.photo_service import PhotoService

router = APIRouter(prefix="/photos", tags=["Photos"], )


@router.post("/infer", response_model=PhotoAnalysisResponse, summary="Receive a photo for processing", )
@inject
async def inspect_photo(
        photo: Annotated[UploadFile, File(...)],
        photo_service: Annotated[PhotoService, Depends(Provide[Container.photo_service])],
) -> PhotoAnalysisResponse:
    return await photo_service.inspect_photo(photo)
