## Overview

This is example projects for Background Jobs implementation in Backend using .NET

## Table of Contents

## Background Jobs

### What

> Background jobs are tasks or processes that are designed to run asynchronously from the main user-facing application. They run independently and can be scheduled or triggered based on specific events.

### Where

- Notifications API ( sending emails, sms)
- Media API ( image compressing, video processing )
- Long-running calculations
- Batch jobs for large data sets
- Scheduled maintenance tasks

### Why

- **Async Communication** - asynchronous communication isn't complete without background jobs
- **Resiliency** - retry policies are easily integrated
- **Resource Management** - Best for resource management to not use all resources at the same time
- **Prioritization** - background jobs allow prioritization of tasks to use resources efficiently
- **Scaling** - we can use different resources for receiving requests and processing them

### How

There are number of features that must be implemented for the full background jobs management and there few patterns for systems that deal with background jobs

[Worker/Scheduler pattern](./docs/worker-scheduler.md)

## Getting Started

### Requirements

- .NET 8 SDK
- Any IDE that supports .NET and C#

## Project Structure

## License

