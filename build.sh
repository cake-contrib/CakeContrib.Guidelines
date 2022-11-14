#!/bin/bash
dotnet tool restore

dotnet cake recipe.cake "$@"