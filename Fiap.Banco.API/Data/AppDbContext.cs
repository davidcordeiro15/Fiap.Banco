using Fiap.Banco.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Fiap.Banco.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Bancos> Bancos => Set<Bancos>();
    public DbSet<Cliente> ClientesBanco => Set<Cliente>();
    public DbSet<Agencia> AgenciaBanco => Set<Agencia>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Contratacao> Contratacoes => Set<Contratacao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Bancos>(entity =>
        {
            entity.ToTable("Bancos");
            entity.HasKey(x => x.idBanco);
            entity.Property(x => x.idBanco).UseIdentityColumn();
            entity.Property(x => x.nomeBanco).HasMaxLength(200).IsRequired();
            entity.Property(x => x.CEP).HasMaxLength(20).IsRequired();
            entity.Property(x => x.dtCriacao).IsRequired();
        });

        modelBuilder.Entity<Agencia>(entity =>
        {
            entity.ToTable("AgenciaBanco");
            entity.HasKey(x => x.idAgencia);
            entity.Property(x => x.idAgencia).UseIdentityColumn();
            entity.Property(x => x.nmEndereco).HasMaxLength(250).IsRequired();
            entity.Property(x => x.cep).HasMaxLength(20).IsRequired();
            entity.HasMany(x => x.Clientes)
                  .WithOne(x => x.Agencia)
                  .HasForeignKey(x => x.idAgencia)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("ClientesBanco");
            entity.HasKey(x => x.idCliente);
            entity.Property(x => x.idCliente).UseIdentityColumn();
            entity.Property(x => x.nmCliente).HasMaxLength(250).IsRequired();
            entity.HasDiscriminator<string>("TipoCliente")
                  .HasValue<PessoaFisica>("PF")
                  .HasValue<PessoaJuridica>("PJ");
        });

        modelBuilder.Entity<PessoaFisica>(entity =>
        {
            entity.Property(x => x.CPF).HasMaxLength(11).IsRequired();
            entity.Property(x => x.DataNascimento).IsRequired();
            entity.HasIndex(x => x.CPF).IsUnique();
        });

        modelBuilder.Entity<PessoaJuridica>(entity =>
        {
            entity.Property(x => x.CNPJ).HasMaxLength(14).IsRequired();
            entity.Property(x => x.RazaoSocial).HasMaxLength(250).IsRequired();
            entity.HasIndex(x => x.CNPJ).IsUnique();
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("Produtos");
            entity.HasKey(x => x.idProduto);
            entity.Property(x => x.idProduto).UseIdentityColumn();
            entity.Property(x => x.nmProduto).HasMaxLength(250).IsRequired();
            entity.Property(x => x.Descricao).HasMaxLength(1000);
            entity.HasDiscriminator<string>("TipoProduto")
                  .HasValue<Emprestimo>("EMPRESTIMO")
                  .HasValue<MaquinaDeCartao>("MAQUINA_CARTAO")
                  .HasValue<ReceberSalario>("RECEBER_SALARIO");
        });

        modelBuilder.Entity<Emprestimo>(entity =>
        {
            entity.Property(x => x.ValorSolicitado).HasColumnType("NUMBER(18,2)");
            entity.Property(x => x.Parcelas);
        });

        modelBuilder.Entity<MaquinaDeCartao>(entity =>
        {
            entity.Property(x => x.VolumeMensalEstimado).HasColumnType("NUMBER(18,2)");
            entity.Property(x => x.TaxaPercentual).HasColumnType("NUMBER(18,2)");
        });

        modelBuilder.Entity<ReceberSalario>(entity =>
        {
            entity.Property(x => x.EmpresaConveniada).HasMaxLength(250);
            entity.Property(x => x.SalarioMensal).HasColumnType("NUMBER(18,2)");
        });

        modelBuilder.Entity<Contratacao>(entity =>
        {
            entity.ToTable("Contratacoes");
            entity.HasKey(x => x.idContratacao);
            entity.Property(x => x.idContratacao).UseIdentityColumn();
            entity.Property(x => x.Status).HasConversion<string>().HasMaxLength(20).IsRequired();
            entity.Property(x => x.TipoProduto).HasMaxLength(50).IsRequired();
            entity.Property(x => x.MensagemProcessamento).HasMaxLength(1000);
            entity.Property(x => x.DataCriacao).IsRequired();
            entity.Property(x => x.DataAtualizacao);

            entity.HasOne(x => x.Cliente)
                  .WithMany(x => x.Contratacoes)
                  .HasForeignKey(x => x.idCliente)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Agencia)
                  .WithMany(x => x.Contratacoes)
                  .HasForeignKey(x => x.idAgencia)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Produto)
                  .WithMany()
                  .HasForeignKey(x => x.idProduto)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
