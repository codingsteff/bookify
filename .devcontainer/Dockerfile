ARG VARIANT="9.0-bookworm"
FROM mcr.microsoft.com/devcontainers/dotnet:1-${VARIANT}

# Install dotnet tools and add it to path
RUN su vscode -c "dotnet tool install --global dotnet-ef" 2>&1
ENV PATH $PATH:/home/vscode/.dotnet/tools