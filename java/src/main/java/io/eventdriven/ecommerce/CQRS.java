package io.eventdriven.ecommerce;

import io.eventdriven.ecommerce.verticalslices.products.findbyid.ProductDetails;
import io.eventdriven.ecommerce.verticalslices.products.get.ProductShortInfo;

import java.util.List;
import java.util.Optional;

public class CQRS {
  // 🤔 CQRS

  // C
  record RegisterProduct() {
  }

  record UpdateProduct() {
  }

  // Q
  public record FindProductById() {
  }

  public record GetProducts() {
  }
  // R S


  // Perform a business operation but do not return data.
  interface CommandHandler<Command> {
    void handle(Command command);
  }

  // Get the data, but do not perform any side effects
  interface QueryHandler<Query, Result> {
    Result handle(Query query);
  }


  // Examples of command handlers
  class RegisterProductHandler implements CommandHandler<RegisterProduct> {
    @Override
    public void handle(RegisterProduct registerProduct) {
      throw new RuntimeException("We'll get to that later");
    }
  }

  class UpdateProductHandler implements CommandHandler<UpdateProduct> {
    @Override
    public void handle(UpdateProduct registerProduct) {
      throw new RuntimeException("We'll get to that later");
    }
  }


  // Examples of query handlers
  class FindProductByIdHandler implements QueryHandler<FindProductById, Optional<ProductDetails>> {
    public Optional<ProductDetails> handle(FindProductById query) {
      throw new RuntimeException("We'll get to that later");
    }
  }

  class GetProductsHandler implements QueryHandler<GetProducts, List<ProductShortInfo>> {
    @Override
    public List<ProductShortInfo> handle(GetProducts getProducts) {
      throw new RuntimeException("We'll get to that later");
    }
  }

  class ProductsApplicationService {
    public void handle(RegisterProduct registerProduct) {
      throw new RuntimeException("We'll get to that later");
    }

    public void handle(UpdateProduct registerProduct) {
      throw new RuntimeException("We'll get to that later");
    }
  }

  class ProductsQueryService {
    public Optional<ProductDetails> handle(FindProductById query) {
      throw new RuntimeException("We'll get to that later");
    }

    public List<ProductShortInfo> handle(GetProducts getProducts) {
      throw new RuntimeException("We'll get to that later");
    }
  }
}
