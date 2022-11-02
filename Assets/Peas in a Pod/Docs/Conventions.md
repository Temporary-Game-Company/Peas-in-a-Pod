1. Use pascal casing ("PascalCasing") when naming a class, record, or struct
2. Use camel casing ("camelCasing") when naming private or internal fields, and prefix them with _.
    - Method parameters too.
    - If static, use s_prefix instead.
    - If thread static, use t_prefix instead.
3. Use descriptive names for all variables.
4. Use implicit typing ('var') when only for local variables and when type is obvious.
5. Don't expose anything globally. Dependency Injection is your friend.
6. **Document your code**