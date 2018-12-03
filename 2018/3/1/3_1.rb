lines = File.readlines('input.txt')
cloth = Hash.new
claims = 0


lines.each do |line|
  #2 @ 153,186: 20x22
  arr = line.split(" ")
  coord = arr[2]
  area = arr[3]

  arr1 = coord.split(',')
  x = Integer(arr1[0])
  y = Integer(arr1[1].chomp(':'))

  arr2 = area.split('x')
  w = Integer(arr2[0])
  l = Integer(arr2[1])

  w.each do |w|
    l.each do |l|
      hashkey = (x + w) + '_' + (y + l)

      if(cloth[hashkey].nil?)
        cloth[hashkey] = 1
      else
        claims += 1
        cloth[hashkey] += 1
      end
    end
  end
end

puts claims
