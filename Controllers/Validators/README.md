# Validators

Use one validator per request DTO in this folder.

Naming convention:
- DTO: `<Feature><Action>RequestDto`
- Validator: `<Feature><Action>RequestDtoValidator`

Examples:
- `RabbitMqPublishRequestDto` -> `RabbitMqPublishRequestDtoValidator`

Register validators in `Program.cs` using assembly scanning:
- `builder.Services.AddValidatorsFromAssemblyContaining<SomeValidator>();`
