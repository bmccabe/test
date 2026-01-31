Feature: Monitor Power Control
    As a system administrator
    I want to control monitor power states
    So that I can manage power consumption

    Scenario: Turn off a monitor that is on
        Given a monitor is turned on
        When I turn off the monitor
        Then the monitor should be off

    Scenario: Turn on a monitor that is off
        Given a monitor is turned off
        When I turn on the monitor
        Then the monitor should be on

    Scenario: Check initial monitor state
        Given a new monitor instance
        Then the monitor should be on by default
