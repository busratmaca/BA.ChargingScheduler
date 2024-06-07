#Charging Scheduler

Charging Scheduler is a program designed to optimize the charging schedule of electric vehicles based on user preferences and car data. The goal is to minimize the energy bill while ensuring the car is charged to the desired level by the user's leaving time.

## Table of Contents

- [Introduction](#introduction)
- [Background](#background)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)


## Introduction

we optimize the charging of electric cars based on user preferences to stabilize the grid. This program generates a charging schedule that minimizes the energy bill while meeting user requirements.

## Background

For this exercise, market demands are ignored. We focus on:
- User Settings
- Car Data
- Charging Schedule

## Features

- **User Settings Configuration**: Allows users to set their desired state of charge, leaving time, direct charging percentage, and tariffs.
- **Car Data Integration**: Includes charge power, battery capacity, and current battery level.
- **Charging Schedule Optimization**: Generates an optimal charging schedule based on the provided data.

## Installation

### Requirements

- .NET 8

### Steps

1. Clone the repository:
    ```bash
    git clone https://github.com/busratmaca/BA.ChargingScheduler.git
    ```

2. Navigate to the project directory:
    ```bash
    cd BA.ChargingScheduler
    ```

3. Build the project:
    ```bash
    dotnet build
    ```

## Usage

1. http://localhost:5209/swagger/index.html

[example files for charging](https://github.com/user-attachments/files/15739251/test.xlsx)
