# Maintenance

The service is part of the TERRA platform offered through an existing deployment, following the TERRA release and deployment procedures over a managed infrasrtucture along with the maintenance activities that are scheduled within the platform. The purpose of this section is not to detail the maintenance activities put in place by the TERRA team.

## Healthchecks

The service offers an explicit healthcheck endpoint.

An example response that returns 200 OK for healthy state is:
```json
[
    {
        "status": "healthy"
    }
]
```

## Verions & Updates

The service follows the versioning and update scheme (Semantic Versions) that the [AI Model Registry service](https://github.com/terra-horizon/ai-model-registry) supports.

## Troubleshooting

Troubleshooting is primarily done through the logging mechanisms that are available and are described in the respective [logging](logging.md) section.