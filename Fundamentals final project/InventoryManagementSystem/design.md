```mermaid
flowchart TD
    Start((Start)) --> Menu[Display Main Menu]
    Menu --> Choice{User Choice?}
    
    Choice -- 1 --> View[View All Products]
    Choice -- 2 --> Add[Enter Product Details]
    Choice -- 3 --> Stock[Enter Stock Update]
    Choice -- 4 --> Remove[Enter Product Name]
    Choice -- 5 --> End((End))
    
    Add --> ValidateAdd{Valid Input?}
    ValidateAdd -- Yes --> SaveProduct[Add New Product]
    ValidateAdd -- No --> ErrorAdd[Show Error Message]
    
    Stock --> ValidateStock{Valid Update?}
    ValidateStock -- Yes --> UpdateStock[Update Stock Count]
    ValidateStock -- No --> ErrorStock[Show Error Message]
    
    Remove --> ValidateRemove{Has Stock?}
    ValidateRemove -- No --> DeleteProduct[Remove Product]
    ValidateRemove -- Yes --> ErrorRemove[Show Error Message]
    
    View --> Menu
    SaveProduct --> Menu
    ErrorAdd --> Menu
    UpdateStock --> Menu
    ErrorStock --> Menu
    DeleteProduct --> Menu
    ErrorRemove --> Menu