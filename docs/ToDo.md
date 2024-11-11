# Here are my future plans for this application:

## Write VueJS app for drawing tree view or D3 structure, and use this client to retrieve child nodes with pagination

        async loadHeadUnit(headId)
        {
            const response = await axios.get(`/ api / organization /${ headId}`);
            this.headUnit = response.data;
        }

        // Load child nodes with pagination
        async loadChildUnits(parentId, pageNumber = 1, pageSize = 10)
        {
            const response = await axios.get('/api/organization/children', {
        params: { parentId, pageNumber, pageSize }
    });
    this.childUnits = response.data.items;
    this.totalPages = response.data.totalPages;
}

## Now in this application there are two independent Mongo indexes binded to Type and Name fields, i have to check performance, requirements and maybe use one compound index, depends on frontend tasks, later
## Move code down below (from Program.cs) to background service
 var mongoIndexMigrator = app.Services.GetRequiredService<IOrganizationService>();
            var logger = app.Services.GetService<ILogger>();
            mongoIndexMigrator.EnsureIndexesCreated();
            logger.LogInformation($"Mongo Indexes Migration Status - OK!");

-------------------------------------------------------------------------
Each department must have a person in charge (like a VP or manager).
Only certain roles can be added at certain levels in the hierarchy.
The organization tree will be stored in a database.
----------------------
Constraints and Validations:

Enforce that certain levels (departments) must have a person in charge and cannot be created without one

Provide API endpoints for:

Adding, updating, and deleting departments and people.
Searching for people by department or role
-------------------------
