from typing import Annotated

from fastapi import APIRouter, Depends, File, UploadFile

from dependencies.services import get_photo_service
from schemas.photos import PhotoAnalysisResponse
from services.photo_service import PhotoService

router = APIRouter(prefix="/photos", tags=["Photos"], )


@router.post("/infer", response_model=PhotoAnalysisResponse, summary="Receive a photo for processing", )
async def inspect_photo(photo: Annotated[UploadFile, File(...)],
        photo_service: Annotated[PhotoService, Depends(get_photo_service)], ) -> PhotoAnalysisResponse:
    return await photo_service.inspect_photo(photo)
