# Stick Blast – Case Implementation for Garawell Games

## Overview
This project is a case implementation of **Stick Blast** for **Garawell Games**. It covers the core logic and systems related to grid generation, drag-and-drop mechanics for shape placement, and dynamic updates to the grid model.

---

## Progress Report

### 06.05.2025
- Project start
- Added necessary packages and performed initial installations
- Project planning and roadmap creation
- Grid system implementation begins
  - `GridCell`, `GridGap` initial implementations
- Began implementing `DraggableObject` system

---

### 07.05.2025
- Imported relevant assets into the project
- Created references for draggable objects
- Continued development of grid creation algorithm

---

### 08.05.2025
- Implemented `DraggableObjectView`
- Completed dragging and shape placement logic
- Refactored grid creation algorithm for flexibility and performance
- Refactored dragging logic for clarity and maintainability

---

### 09.05.2025
- Implemented grid cell highlighting during shape hover
- Started implementation of the **Model Layer**
  - Created `GridCellData`
  - Linked runtime data with `GameModel`
- Moved highlight logic to a dedicated **Mediator** class
- Started implementation of grid cleanup logic
  - Removing corner and square tiles after a column/row is completed
  - Basic functionality working, though currently buggy

---

## Notes
This project is a work-in-progress and follows an iterative implementation approach. Future updates will address current bugs and expand the game logic.

- The project is built using a fully functional **MVC (Model-View-Controller)** architecture for clear separation of concerns.
- Strong emphasis is placed on **encapsulation** to ensure clean and maintainable code.
- **Dependency Injection** is implemented to promote modularity and testability.
  - The [Bit34Games Injector](https://github.com/bit34games/injector) library is used for handling dependency resolution efficiently.

## TO DO

- Fix bugs related to clearing completed rows and columns
- Implement scoring logic upon successful shape placement
- Load pre-defined levels using **ScriptableObjects**
- Improve user interface elements and feedback
- General polishing and performance optimization
