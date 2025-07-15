---
mode: 'ask'
description: 'Perform a REST API security review'
---
Perform a REST API security review and provide a TODO list of security issues to address.

* Ensure all endpoints are protected by authentication and authorization
* Validate all user inputs and sanitize data
* Expose only DTOs instead of entities (Prevent leaking of sensitive domain model information)

Return the TODO list in a Markdown format, grouped by priority and issue type.