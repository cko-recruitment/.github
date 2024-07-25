Architectural decisions:
- i didn't implement any authentication or authorization mechanism for simplicity
- I used an in memory payments repository as per instructions
- acceptance tests test the API functionality mocking any external dependecies. For simplicity, I set up general mock external dependency. In practice i would have set up mocks per test
- I wrote separate contract classes for API and client/persistence layer to make those layers independent of each other
- PaymentsRepository is only unit tested since it's an in memory repository
- I used wiremock to mock Acquiring Bank since i couldn't find any simulator and wiremock was mentioned as an acceptable solution
- i used snake case json formatting for API and clients to match the example provided

To run tests:
- all tests can be run from within Visual Studio
- to execute integration tests and e2e tests one need to run before `docker-compose -f docker-compose.yml -f docker-compose.local.yml up` from within repo root folder. This will run wiremock and expose it locally