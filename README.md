# JSON Comparison Tool

A modern Blazor WebAssembly Progressive Web App (PWA) for comparing JSON files containing arrays of objects based on configurable fields.

## Features

- 🔍 **Configurable Comparison**: Compare objects based on specific fields you choose
- 📊 **Detailed Analysis**: Comprehensive reports showing matches, differences, and unique objects
- 💾 **Multiple Export Formats**: Download results in JSON, CSV, or HTML format
- ⚙️ **Flexible Options**: Configure case sensitivity, array order handling, and field inclusion
- 🌐 **Works Offline**: Progressive Web App that works entirely in your browser
- 📱 **Cross-Platform**: Works on any device with a modern web browser
- 🎯 **Nested Field Support**: Use dot notation for nested properties (e.g., `user.profile.name`)

## Architecture

The solution is structured into three separate projects for maintainability and future enhancement:

```
src/
├── JsonComparisonTool.Core/     # Business logic and models
├── JsonComparisonTool.Shared/   # Blazor components and abstractions  
└── JsonComparisonTool.Web/      # Blazor WebAssembly PWA
```

### Project Separation Benefits

- **Core**: Reusable business logic that can be shared across different UI frameworks
- **Shared**: UI components that can be used in both web and desktop applications
- **Web**: Web-specific implementation with PWA capabilities

## Getting Started

### Prerequisites

- .NET 8 SDK or later
- Modern web browser (Chrome, Firefox, Safari, Edge)

### Running the Application

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd JsonComparisonTool
   ```

2. Build and run the application:
   ```bash
   dotnet run --project src/JsonComparisonTool.Web
   ```

3. Open your browser and navigate to `https://localhost:5001` (or the URL shown in the terminal)

### Publishing for Production

To publish the application for deployment:

```bash
dotnet publish src/JsonComparisonTool.Web -c Release -o publish/
```

The published files can be hosted on any static web server (GitHub Pages, Netlify, Azure Static Web Apps, etc.).

## Usage

1. **Upload JSON Files**: Select two JSON files containing arrays of objects
2. **Configure Comparison**: 
   - Specify fields to compare (leave empty to compare all fields)
   - Use dot notation for nested fields (e.g., `user.profile.name`)
   - Configure case sensitivity and other options
3. **Compare**: Click "Compare JSON Files" to analyze differences
4. **View Results**: Review detailed comparison results with summary statistics
5. **Export**: Download results in JSON, CSV, or HTML format

### Example JSON Structure

```json
[
  {
    "id": 1,
    "name": "John Doe",
    "email": "john@example.com",
    "profile": {
      "age": 30,
      "city": "New York"
    }
  },
  {
    "id": 2,
    "name": "Jane Smith", 
    "email": "jane@example.com",
    "profile": {
      "age": 25,
      "city": "Los Angeles"
    }
  }
]
```

### Configuration Options

- **Comparison Fields**: Comma-separated list of fields to compare (e.g., `id, name, profile.age`)
- **Case Sensitive**: Whether string comparisons should be case-sensitive
- **Ignore Array Order**: Whether to ignore the order of elements in arrays
- **Ignore Extra Fields**: Whether to ignore fields not specified in comparison fields
- **Output Format**: Choose between JSON, CSV, or HTML for exported results

## PWA Features

- **Offline Capability**: Works without internet connection after initial load
- **Install Prompt**: Can be installed as a desktop app on supported browsers
- **Responsive Design**: Optimized for both desktop and mobile devices
- **Service Worker**: Caches application for offline use

## Development

### Project Structure

```
JsonComparisonTool/
├── JsonComparisonTool.sln                    # Solution file
└── src/
    ├── JsonComparisonTool.Core/
    │   ├── Models/                            # Data models
    │   │   ├── ComparisonConfig.cs
    │   │   └── ComparisonResult.cs
    │   └── Services/
    │       └── JsonComparator.cs              # Core comparison logic
    ├── JsonComparisonTool.Shared/
    │   ├── Components/                        # Reusable Blazor components
    │   │   ├── ConfigurationComponent.razor
    │   │   ├── FileUploadComponent.razor
    │   │   └── ResultsComponent.razor
    │   └── Services/
    │       └── IFileService.cs                # File handling abstraction
    └── JsonComparisonTool.Web/
        ├── Pages/                             # Blazor pages
        │   ├── Index.razor
        │   └── Compare.razor
        ├── Services/
        │   └── WebFileService.cs              # Web-specific file handling
        └── wwwroot/                           # Static web assets
            ├── css/app.css
            ├── js/app.js
            ├── manifest.json                  # PWA manifest
            └── service-worker.js              # Service worker for offline
```

### Adding New Features

To extend the application:

1. **Add business logic** to `JsonComparisonTool.Core`
2. **Create reusable components** in `JsonComparisonTool.Shared`
3. **Implement web-specific features** in `JsonComparisonTool.Web`

### Future Enhancement Possibilities

The modular architecture supports easy addition of:

- Desktop application using MAUI Blazor Hybrid
- Console application for batch processing
- Additional export formats
- Advanced comparison algorithms
- Custom field transformation rules

## Technology Stack

- **Frontend**: Blazor WebAssembly
- **Styling**: CSS3 with modern design patterns
- **JSON Processing**: System.Text.Json
- **PWA**: Service Worker, Web App Manifest
- **Build**: .NET 8 SDK

## Browser Support

- Chrome 80+
- Firefox 74+
- Safari 13.1+
- Edge 80+

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
