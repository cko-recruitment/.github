Architectural decisions:
- i didn't implement any authentication or authorization mechanism for simplicity
- I used an in memory payments repository as per instructions
- acceptance tests test the API functionality mocking any external dependecies. For simplicity, I set up general mock external dependency. In practice i would have set up mocks per test 