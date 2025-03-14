### Pokemon TCG App

<img src="https://img.shields.io/badge/Visual_Studio_Code-0078D4?style=for-the-badge&logo=visual%20studio%20code&logoColor=white" /> <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white"/> <img src="https://img.shields.io/badge/ChatGPT-74aa9c?style=for-the-badge&logo=openai&logoColor=white"/> [![forthebadge](http://forthebadge.com/images/badges/built-with-love.svg)](http://forthebadge.com) 



A Blazor WebAssembly application that allows users to search, view, and manage their Pokemon Trading Card Game collection. This application integrates with the Pokemon TCG API to provide access to thousands of cards, and allows users to save their favorite cards with personal notes, condition ratings, and market prices.

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Installation](#installation)
- [Project Structure](#project-structure)
- [Test Scenarios](#test-scenarios)
- [Solution Approach](#solution-approach)
- [Conceptual and Methodological Choices](#conceptual-and-methodological-choices)
- [Usage](#usage)
- [API Key](#api-key)
- [Contributing](#contributing)
- [License](#license)


## Features

- **Card Search**: Search for Pokemon cards by name
- **Infinite Scrolling**: Load more cards with pagination
- **Card Details**: View detailed information about each card
- **Favorites Management**: Add cards to favorites and remove them
- **Collection Notes**: Add personal notes, condition ratings, and market prices to favorite cards
- **Persistent Storage**: Favorites are saved in the browser's localStorage
- **Responsive Design**: Works on desktop and mobile devices


## Technologies Used

- **Blazor WebAssembly**: For client-side web application development
- **.NET 8**: Latest .NET framework
- **Pokemon TCG API**: External API for Pokemon card data
- **LocalStorage**: For client-side data persistence
- **Bootstrap 5**: For responsive UI components
- **xUnit**: For unit testing
- **Moq**: For mocking in unit tests


## Installation

1. Clone the repository:

```plaintext
git clone https://github.com/yourusername/pokemon-tcg-app.git
```


2. Navigate to the project directory:

```plaintext
cd pokemon-tcg-app
```


3. Update the API key in `Services/PokemonTcgService.cs`:

```csharp
private readonly string _apiKey = "YOUR_API_KEY"; // Replace with your API key
```


4. Run the application:

```plaintext
dotnet run
```


5. Open your browser and navigate to:

```plaintext
https://localhost:7186
```




## Project Structure

```plaintext
PokemonTcgApp/
├── Components/                # Reusable UI components
│   ├── CardDetail.razor       # Card detail view component
│   ├── PokemonCardComponent.razor # Card grid item component
│   └── SearchBar.razor        # Search input component
├── Models/                    # Data models
│   ├── FavoriteCard.cs        # Model for favorite cards with user data
│   └── PokemonCard.cs         # Model for Pokemon cards from API
├── Pages/                     # Application pages
│   ├── Favorites.razor        # Favorites management page
│   └── Index.razor            # Main search page
├── Services/                  # Business logic services
│   ├── FavoritesService.cs    # Service for managing favorites
│   └── PokemonTcgService.cs   # Service for API integration
├── Tests/                     # Unit tests
│   ├── FavoritesServiceTests.cs # Tests for favorites service
│   └── PokemonTcgServiceTests.cs # Tests for API service
├── wwwroot/                   # Static web assets
│   ├── css/                   # Stylesheets
│   └── index.html             # Main HTML file
├── App.razor                  # Root component
├── MainLayout.razor           # Main layout component
├── NavMenu.razor              # Navigation menu component
├── Program.cs                 # Application entry point
└── _Imports.razor             # Razor imports
```

## Test Scenarios

### PokemonTcgServiceTests.cs

This test file covers the following scenarios:

1. **SearchCards_ReturnsCards_WhenApiCallSucceeds**

1. Verifies that the `SearchCards` method correctly returns a list of Pokemon cards when the API call is successful
2. Checks that the returned data is properly deserialized and mapped to the `PokemonCard` model
3. Validates that specific properties like ID and Name are correctly extracted



2. **SearchCards_ReturnsEmptyList_WhenApiCallFails**

1. Ensures that the `SearchCards` method returns an empty list when the API call fails
2. Verifies that the application gracefully handles API errors without crashing



3. **GetCardById_ReturnsCard_WhenApiCallSucceeds**

1. Tests that the `GetCardById` method correctly returns a single Pokemon card when the API call is successful
2. Validates that the card properties are correctly mapped from the API response



4. **GetCardById_ReturnsNull_WhenApiCallFails**

1. Ensures that the `GetCardById` method returns null when the API call fails
2. Verifies that the application handles API errors gracefully when fetching a specific card





### FavoritesServiceTests.cs

This test file covers the following scenarios:

1. **GetFavoritesAsync_ReturnsEmptyList_WhenNoFavoritesExist**

1. Verifies that the `GetFavoritesAsync` method returns an empty list when no favorites exist in localStorage
2. Ensures that the application handles the case of a new user with no favorites



2. **GetFavoritesAsync_ReturnsFavorites_WhenFavoritesExist**

1. Tests that the `GetFavoritesAsync` method correctly returns the list of favorite cards when they exist in localStorage
2. Validates that the favorites are properly deserialized from JSON



3. **AddFavoriteAsync_AddsFavorite_WhenCardIsNotAlreadyFavorite**

1. Ensures that the `AddFavoriteAsync` method correctly adds a card to favorites
2. Verifies that the card is saved to localStorage
3. Checks that the card is accessible in the favorites list after adding



4. **RemoveFavoriteAsync_RemovesFavorite_WhenCardIsFavorite**

1. Tests that the `RemoveFavoriteAsync` method correctly removes a card from favorites
2. Verifies that the card is removed from localStorage
3. Ensures that the card is no longer in the favorites list after removal



5. **UpdateFavoriteAsync_UpdatesFavorite_WhenCardIsFavorite**

1. Verifies that the `UpdateFavoriteAsync` method correctly updates a favorite card's information
2. Tests that user-specific data (notes, condition, market price) is properly updated
3. Ensures that the updated information is saved to localStorage





## Solution Approach

### Criterion 1: Justification of the Solution Method

The Pokemon TCG App was developed using a component-based architecture with Blazor WebAssembly, which offers several advantages for this type of application:

1. **Single Page Application (SPA)**: Blazor WebAssembly allows for a responsive SPA experience without page reloads, providing a smooth user experience when searching for and viewing Pokemon cards.
2. **C# Throughout**: Using C# for both frontend and backend logic eliminates the need for context switching between languages, increasing development efficiency and code consistency.
3. **Component Reusability**: The application is built with reusable components (like `PokemonCardComponent` and `SearchBar`), which promotes code reuse and maintainability.
4. **Client-Side Processing**: By processing data on the client side, the application reduces server load and provides immediate feedback to user actions, such as filtering and sorting cards.
5. **Progressive Enhancement**: The application is designed to work even with limited connectivity, storing favorites locally in the browser's localStorage.
6. **Testability**: The service-based architecture allows for easy unit testing of business logic, as demonstrated by the comprehensive test coverage.
7. **Incremental Development**: The project was developed incrementally, starting with core functionality (API integration) and progressively adding features (favorites management, UI enhancements), allowing for regular validation and adjustment.


## Conceptual and Methodological Choices

### Criterion 2: Justification of Conceptual and Methodological Choices

1. **Service-Based Architecture**:

1. **Why**: Separating business logic into services (`PokemonTcgService` and `FavoritesService`) creates a clear separation of concerns.
2. **Benefit**: This approach makes the code more maintainable, testable, and allows for easier future enhancements.



2. **Repository Pattern for API Integration**:

1. **Why**: The `PokemonTcgService` implements a repository pattern to abstract the API calls.
2. **Benefit**: This abstraction shields the application from changes in the external API and centralizes error handling.



3. **Client-Side Storage with localStorage**:

1. **Why**: Using browser's localStorage for favorites instead of a server database.
2. **Benefit**: This approach eliminates the need for user authentication while still providing persistence between sessions, and works offline.



4. **Responsive Design First**:

1. **Why**: Designing the UI to be responsive from the start rather than as an afterthought.
2. **Benefit**: Ensures a consistent experience across devices and reduces the need for device-specific code.



5. **Lazy Loading with "Load More" Pattern**:

1. **Why**: Implementing pagination with a "Load More" button instead of traditional page numbers.
2. **Benefit**: Provides a more seamless user experience and reduces initial load time by fetching only the data needed.



6. **Scroll Position Preservation**:

1. **Why**: Adding JavaScript interop to maintain scroll position when loading more cards.
2. **Benefit**: Enhances user experience by preventing the page from jumping to the top when new content is loaded.



7. **Comprehensive Testing Strategy**:

1. **Why**: Creating unit tests for core services with mocked dependencies.
2. **Benefit**: Ensures reliability of critical functionality and facilitates future refactoring.



8. **Debounced Search**:

1. **Why**: Implementing debounce in the search functionality.
2. **Benefit**: Reduces unnecessary API calls while typing, improving performance and user experience.





## Usage

### Search Page

1. Enter a Pokemon card name in the search bar
2. Browse through the search results
3. Click on a card to view its details
4. Click the "+" button to add a card to favorites
5. Click "Load More" to see additional cards


### Favorites Page

1. Navigate to "My Favorites" using the navigation menu
2. View all your favorite cards
3. Click on a card to view its details and edit your notes
4. Add condition, market price, and personal notes
5. Click "Save Notes" to update your information
6. Click "Remove from Favorites" to remove a card from your collection


## API Key

To use this application, you need to obtain an API key from the [Pokemon TCG API](https://dev.pokemontcg.io/). Once you have your key, update the `_apiKey` field in `Services/PokemonTcgService.cs`.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
