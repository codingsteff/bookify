---
mode: agent
---

Scan the use cases in the `src/Bookify.Application/[FEATURE]` project, and generate the unit tests for them.

Ask for the feature folder and for the use case if not provided.

The Unit Tests should be written in the `test/Bookify.Application.UnitTests/[FEATURE]` project, and you should create a new file for each use case that you are testing.

You should use the @ReserveBookingTests.cs class as a baseline of what a good test should look like.

Pay attention to the arrange-act-assert structure of the tests, and the naming convention for the test cases.

We're using NSubstitute for mocking and it should reaming that way.