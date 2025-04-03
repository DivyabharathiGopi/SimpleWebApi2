using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleApi.Models.Domain;
using SimpleApi.Models.DTOs;
using System.Linq;
using System.Collections.Generic;


[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase

{

       private static List<Product>  products=new List<Product>
    {
        new Product
        {
            Id=Guid.NewGuid(),
            Name="Laptop",
            Description="Electronic"
        },

        new Product
        {
            Id=Guid.NewGuid(),
            Name="TV",
            Description="Electronic"
        }
    };

    private ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id=product.Id,
            Name=product.Name,
            Description=product.Description
        };
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<ProductDto>> Get()
    {
        var productDtos=products.Select(p=>MapToDto(p)).ToList();
        return  Ok(productDtos);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<ProductDto> Get([FromRoute] Guid id)
    {
        var ExistingProduct = products.FirstOrDefault(x=>x.Id==id);
        if(ExistingProduct==null)
        {
            return NotFound("Product Not Found!");
        }
        return Ok(MapToDto(ExistingProduct));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Product> Post([FromBody] CreateProductDto createProductDto)
    {
        if(createProductDto==null)
        {
            return BadRequest("Invalid or missing Data");
        }

        var product= new Product
        {
            Id=Guid.NewGuid(),
            Name=createProductDto.Name,
            Description=createProductDto.Description
        };
       
        products.Add(product);
        return CreatedAtAction(nameof(Get), new {id=product.Id},MapToDto(product));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    
    public ActionResult Update([FromRoute] Guid id,[FromBody] UpdateProductDto updateProductDto)
    {
       

        var ExistingProduct = products.FirstOrDefault(x=>x.Id==id);
        if(ExistingProduct==null)
        {
            return NotFound();
        }
         if(id!=ExistingProduct.Id)
        {
            return BadRequest();
        }

        ExistingProduct.Name=updateProductDto.Name;
        ExistingProduct.Description=updateProductDto.Description;
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public ActionResult Delete([FromRoute] Guid id)
    {
        var ExistingProduct=products.FirstOrDefault(x=>x.Id==id);
        if(ExistingProduct==null)
        {
            return NotFound();
        }
        products.Remove(ExistingProduct);
        return NoContent();
    }
}